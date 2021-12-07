using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class SeasonSkinClass
{
    public bool is_activated = false;
    public string skin;
    public long time;
}

public class Database : MonoBehaviour
{
    private static SeasonSkinClass seasonSkin;
    public static SeasonSkinClass SeasonSkin { get => seasonSkin; }

    bool isChristmas = false;
    public bool IsChristmas { get => isChristmas; }

    public GameObject errorUI;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeRequest());
    }

    // return authentication header string
    string authenticate(string username, string password)
    {
        // concat user name and password then encode it
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        // authtype basic because the digest authtype is not supported
        auth = "Basic " + auth;
        return auth;
    }

    IEnumerator makeRequest()
    {
        // get authorization header
        string authorization = authenticate("kurage", "8VQ&MXehu!4&S3B4Ts$k");

#if UNITY_EDITOR
        string url = "http://alanparadis.fr/games/kurage/querrySeasonalSkin.php";
#else
        // url to the querry php file
        string url = "https://alanparadis.fr/games/kurage/querrySeasonalSkin.php";
#endif
        // try connect
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // set header
            webRequest.SetRequestHeader("AUTHORIZATION", authorization);
            // get current page for logs
            string[] pages = url.Split('/');
            int page = pages.Length - 1;
            // handle the web request states
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                {
                    Debug.LogError($"Failed to communicate with the server. Error : {webRequest.error}");
                    break;
                }
                case UnityWebRequest.Result.DataProcessingError:
                {
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                }
                case UnityWebRequest.Result.ProtocolError:
                {
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                }
                // request successful
                case UnityWebRequest.Result.Success:
                {
                    // logs
                    Debug.Log("Succesful request !");
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    // check for php error codes
                    var match = Regex.Match(webRequest.downloadHandler.text, @"^(\d): (.*)");
                    if (match.Success)
                    {
                        Debug.LogError($"[Server side error] {match.Groups[2]} (code : {match.Groups[1]})");
                        yield break;
                    }
                    // parse jsonified data
                    seasonSkin = JsonUtility.FromJson<SeasonSkinClass>(webRequest.downloadHandler.text);
                    // get current datetime
                    seasonSkin.time = DateTime.Now.ToBinary();
                    // stor if its Xmas or not
                    isChristmas = seasonSkin.is_activated;
                    // parse to json
                    string json = JsonUtility.ToJson(seasonSkin);
                    // save
                    PlayerPrefs.SetString("SeasonSkin", json);
                    yield break;
                }
            }
        }

        errorUI.SetActive(true);

        // if there is no connection but there is a save
        if (PlayerPrefs.HasKey("SeasonSkin"))
        {
            // get the save
            seasonSkin = JsonUtility.FromJson<SeasonSkinClass>(PlayerPrefs.GetString("SeasonSkin"));
            // if the save is more than a month old, disable the season skin
            if ( seasonSkin.is_activated && (DateTime.Now - DateTime.FromBinary(seasonSkin.time)).TotalDays> 30)
            {
                seasonSkin.is_activated = false;
            }
        }
        else
        {
            // if there is no connection and no save, create empty season skin
            seasonSkin = new SeasonSkinClass();
            seasonSkin.is_activated = false;
            seasonSkin.skin = "";
            seasonSkin.time = DateTime.Now.ToBinary();
        }
        
    }
}
