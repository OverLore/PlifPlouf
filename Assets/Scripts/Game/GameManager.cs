using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public float scrollingSpeed;
    public int maxDepthLayer = 10;

    public static GameManager instance;
    public WaveSpawner waveSpawner;

    [SerializeField] Image pauseBack;

    public bool Paused;

    public int levelToLoad;

    public float timeScale = 1f;

    [Space(10), Header("Stats")]
    public int money = 2000;
    public int maxLives = 5;
    public int lives = 5;
    public System.DateTime nextLifeAt;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI kakiText;

    public bool AdsInitialized = false;


    // score
    [field: SerializeField] private ulong score;
    public ulong Score { get => score; }
    [SerializeField] private uint scoreAdded = 10;

    [SerializeField] Player mplayer;
    public Player player { set { mplayer = value; } get { return GetPlayer(); } }

    [SerializeField] Canvas mlifebarCanvas;
    public Canvas lifebarCanvas { get { return GetLifebarCanvas(); } }

    // combo
    [Space(10), Header("Combo")]
    [SerializeField] private double comboFactor = 1.0;
    private float comboTimer = 0.0f;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float comboTime = 2.5f;
    [SerializeField]
    [Range(5.0f, 10.0f)]
    float comboAngleShake = 7.5f;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float comboFactToMaxScale = 5f;
    [SerializeField]
    [Range(1.0f, 3.0f)]
    float comboTextMaxScale = 2f;

    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI comboText = null;
    Animator comboAnimator;

    public int maxLevelReached = -1;

    [Space(10), Header("Colors")]
    public Color[] colorByLayers;
    public Color[] colorFadeByLayersFrom;
    public Color[] colorFadeByLayersTo;

    private float startVolume = 0.5f;
    private float endVolume = 1.0f;

    public string profileName = "";
    public List<string> profileNames = new List<string>();

    public void LoadProfiles()
    {
        if (PlayerPrefs.HasKey("Profiles"))
        {
            profileNames = PlayerPrefs.GetString("Profiles").Split(',').ToList();
        }

        if (profileNames.Count == 0)
        {
            profileNames.Add("user1");

            profileName = "user1";

            SaveCurrentUser();
            SaveProfiles();
        }
    }

    public void SaveProfiles()
    {
        string str = "";

        foreach (string pn in profileNames)
        {
            str += pn;
            str += ',';
        }

        str = str.Substring(0, str.Length - 1);

        PlayerPrefs.SetString("Profiles", str);
    }

    public void SaveCurrentUser()
    {
        PlayerPrefs.SetString("CurrentUser", profileName);
    }

    public void LoadCurrentUser()
    {
        profileName = PlayerPrefs.GetString("CurrentUser");
    }

    public List<string> GetProfiles()
    {
        return profileNames;
    }

    public bool IsExistingUser(string username)
    {
        return profileNames.Contains(username);
    }

    public void CreateUser(string username)
    {
        profileNames.Add(username);

        SaveProfiles();
    }

    public void ChangeUser(string username)
    {
        profileName = username;

        SaveCurrentUser();
    }

    void LoadStats()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            instance.money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            instance.money = 2000;
            PlayerPrefs.SetInt("Money", instance.money);
        }

        if (PlayerPrefs.HasKey("Lives"))
        {
            instance.lives = PlayerPrefs.GetInt("Lives");
        }
        else
        {
            instance.lives = 5;
            PlayerPrefs.SetInt("Lives", instance.lives);
        }
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("Money", instance.money);
        PlayerPrefs.SetInt("Lives", instance.lives);
    }

    public void ChangeMoney(int _modifier)
    {
        instance.money += _modifier;
        instance.money = Mathf.Clamp(instance.money, 0, 999999);

        PlayerPrefs.SetInt("Money", instance.money);
    }

    public void ChangeLives(int _modifier)
    {
        instance.lives += _modifier;
        instance.lives = Mathf.Clamp(instance.lives, 0, 5);

        if (instance.lives == 5)
        {
            nextLifeAt = System.DateTime.Now;
        }

        PlayerPrefs.SetInt("Lives", instance.lives);
    }

    private void UpdateLivesUI()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            return;
        }

        if (instance.livesText == null)
        {
            instance.livesText = GameObject.Find("LifesText").GetComponent<TextMeshProUGUI>();
        }

        if (instance.lives < instance.maxLives)
        {
            System.TimeSpan diff = instance.nextLifeAt.Subtract(System.DateTime.Now);

            while (diff.TotalSeconds < 0 && instance.lives < instance.maxLives)
            {
                instance.lives++;

                if (instance.lives < instance.maxLives)
                {
                    instance.nextLifeAt = instance.nextLifeAt.AddMinutes(20);
                    diff = instance.nextLifeAt.Subtract(System.DateTime.Now);
                }

                SaveStats();
            }

            if (instance.lives < instance.maxLives && instance.livesText != null)
            {
                instance.livesText.text = $"{instance.lives} ({diff.ToString("mm':'ss")})";
            }
        }
        else
        {
            instance.livesText.text = $"{instance.lives}";

            PlayerPrefs.DeleteKey("nextLifeAt");
        }
    }

    private void GetTexts()
    {
        var textObjects = FindObjectsOfType<TextMeshProUGUI>();
        foreach (var textObject in textObjects)
        {
            if (textObject.name == "Score")
            {
                scoreText = textObject;
            }
            if (textObject.name == "Combo")
            {
                comboText = textObject;
                comboAnimator = comboText.GetComponent<Animator>();
            }
        }
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadProfiles();
        LoadCurrentUser();

        if (PlayerPrefs.HasKey(GameManager.instance.profileName + "maxLevelReached"))
        {
            instance.maxLevelReached = PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached");
        }
        else
        {
            instance.maxLevelReached = -1;
            PlayerPrefs.SetInt(GameManager.instance.profileName + "maxLevelReached", -1);
        }

        if (PlayerPrefs.HasKey("nextLifeAt"))
        {
            long temp = System.Convert.ToInt64(PlayerPrefs.GetString("nextLifeAt"));
            nextLifeAt = System.DateTime.FromBinary(temp);
        }
        //ACHIEVMENT STUFF
        if (!PlayerPrefs.HasKey(GameManager.instance.profileName + "CoinPicked"))
        {
            PlayerPrefs.SetInt(GameManager.instance.profileName + "CoinPicked", 0);
        }
       

        ///
        Application.targetFrameRate = 60;

        GetTexts();

        LoadStats();
    }

    public void LoseLife()
    {
        System.TimeSpan diff = instance.nextLifeAt.Subtract(System.DateTime.Now);

        if (instance.nextLifeAt == null || diff.TotalSeconds < 0)
        {
            instance.nextLifeAt = System.DateTime.Now.AddMinutes(20);
        }

        instance.lives--;

        PlayerPrefs.SetString("nextLifeAt", instance.nextLifeAt.ToBinary().ToString());

        SaveStats();
    }

    private void Update()
    {
        UpdateCombo();

        UpdateLivesUI();

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseLife();
        }

        if (pauseBack == null)
        {
            pauseBack = GameObject.Find("PauseBack")?.GetComponent<Image>();
        }

        if (pauseBack != null)
        {
            if (Paused)
            {
                timeScale = Mathf.Lerp(timeScale, .05f, Time.deltaTime * 5f);
                pauseBack.color = Color.Lerp(pauseBack.color, new Color(0, 0, 0, 185f / 255f), Time.deltaTime * 5f);
                AudioListener.volume = Mathf.Lerp(startVolume, endVolume, Time.deltaTime * 2.0f);
            }
            else
            {
                timeScale = Mathf.Lerp(timeScale, 1f, Time.deltaTime * 5f);
                pauseBack.color = Color.Lerp(pauseBack.color, new Color(0, 0, 0, 0), Time.deltaTime * 5f);
                AudioListener.volume = Mathf.Lerp(endVolume, startVolume, Time.deltaTime * 2.0f);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Paused = !Paused;
            }
        }

        for (int i = 0; i < colorByLayers.Length; i++)
        {
            colorByLayers[i].r = Mathf.Lerp(colorFadeByLayersFrom[i].r, colorFadeByLayersTo[i].r, Mathf.PerlinNoise(Time.time, 0));
            colorByLayers[i].g = Mathf.Lerp(colorFadeByLayersFrom[i].g, colorFadeByLayersTo[i].g, Mathf.PerlinNoise(Time.time, 0));
            colorByLayers[i].b = Mathf.Lerp(colorFadeByLayersFrom[i].b, colorFadeByLayersTo[i].b, Mathf.PerlinNoise(Time.time, 0));
        }
    }

    public Player GetPlayer()
    {
        if (mplayer == null)
        {
            mplayer = GameObject.Find("Player")?.GetComponent<Player>();
        }

        return mplayer;
    }

    public Canvas GetLifebarCanvas()
    {
        if (mlifebarCanvas == null)
        {
            mlifebarCanvas = GameObject.Find("LifebarCanvas")?.GetComponent<Canvas>();
        }

        return mlifebarCanvas;
    }

    public void ChangeMaxLevelReached(int max)
    {
        maxLevelReached = max;
        PlayerPrefs.SetInt(GameManager.instance.profileName + "maxLevelReached", maxLevelReached);
    }

    public void LoadLevel()
    {
        //set pause volume at start
        AudioListener.volume = startVolume;

        LevelManager.instance.StartLevel(levelToLoad);

        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        waveSpawner.LoadLevelWaves(levelToLoad.ToString());

    }

    bool HasPlayer()
    {
        if (player == null)
        {
            player = GameObject.Find("Player")?.GetComponent<Player>();
        }

        return player != null;
    }

    public void ActivateAttackSpeed()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateAttackSpeed();
        }
    }

    public void ActivateAttackDamage()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateAttackDamage();
        }
    }

    public void ActivateShield()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateShield();
        }
    }

    public void ActivateHorizontalShot()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateHorizontalShot();
        }
    }

    public void ActivateShotNumber()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateShotNumber();
        }
    }

    #region SCORE

    public void AddScore()
    {
        IncreaseCombo();
        score += (ulong)((double)scoreAdded * comboFactor);
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    public void AddScore(uint _score)
    {
        IncreaseCombo();
        score += (ulong)((double)_score * comboFactor);
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    public void ResetScore()
    {
        score = 0;
    }

    #endregion

    #region COMBO

    private void UpdateCombo()
    {
        if (comboText == null)
        {
            return;
        }

        if (comboFactor > 1f)
        {
            comboTimer -= Time.deltaTime * instance.timeScale;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
            comboText.enabled = true;
        }
        else
        {
            comboText.enabled = false;
        }
    }

    private void IncreaseCombo()
    {
        comboFactor += 0.05;

        string str;
        if (comboFactor < 10)
        {
            str = $"Combo x{comboFactor.ToString("0.00")} !";
        }
        else if (comboFactor < 100)
        {
            str = $"Combo x{comboFactor.ToString("00.0")} !";
        }
        else if (comboFactor < 1000)
        {
            str = $"Combo x{comboFactor.ToString("000")} !";
        }
        else
        {
            str = $"Combo xtreme !";
        }
        //string str = $"Combo x{comboFactor.ToString("0.###E+00")} !";
        //for(int i = 0; i < comboFactor - 2;i++) {str += "!";}
        comboText.text = str;

        comboAnimator.Play("Combo", -1, 0);


        comboText.transform.rotation =
            Quaternion.Euler(
                new Vector3(0, 0,
                Random.Range(-comboAngleShake, comboAngleShake)));

        float scale = Mathf.Lerp(50, 60,
            Mathf.Clamp((float)comboFactor, 1, comboFactToMaxScale) / comboFactToMaxScale);
        comboText.fontSize = (int)scale;
        //comboText.transform.localScale = new Vector3(scale, scale, 1);
        comboText.transform.localPosition = new Vector3(0f, -120f - 10f *
            Mathf.Clamp((float)comboFactor, 1, comboFactToMaxScale)
            , 0f);


        comboTimer = comboTime;
    }

    private void ResetCombo()
    {
        comboFactor = 1.0;
        comboTimer = comboTime;
    }
    #endregion

    private void OnLevelWasLoaded(int level)
    {
        GetTexts();
    }
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
