using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public struct Line
    {
        public string playerName;
        public int kaki;
        public int medals;
        public int score;
        public int stars;
        public int kills;
    }

    [SerializeField] GameObject linePrefab;
    [SerializeField] bool isLocal;

    float selectedHandleId;
    List<float> handlesTime = new List<float>();
    List<float> handlesY = new List<float>();
    [SerializeField] List<RectTransform> handles;
    [SerializeField] Vector3 handleOffPos;
    [SerializeField] Vector2 handleOffSize;
    [SerializeField] Vector3 handleOnPos;
    [SerializeField] Vector2 handleOnSize;

    public int _level = 0;

    List<Line> lines = new List<Line>();

    private void Start()
    {
        for (int i = 0; i < handles.Count; i++)
        {
            handlesTime.Add(0);
            handlesY.Add(handles[i].localPosition.y);
        }
    }

    public void SetSelectedHandle(int id)
    {
        selectedHandleId = id;
    }

    private void Update()
    {
        for (int i = 0; i < handles.Count; i++)
        {
            handlesTime[i] += ((i == selectedHandleId) ? Time.deltaTime : -Time.deltaTime) * 7.5f;
            handlesTime[i] = Mathf.Clamp01(handlesTime[i]);

            handles[i].localPosition = Vector3.Lerp(new Vector3(handleOffPos.x, handlesY[i], 0), new Vector3(handleOnPos.x, handlesY[i], 0), handlesTime[i]);
            handles[i].sizeDelta = new Vector2(Mathf.Lerp(handleOffSize.x, handleOnSize.x, handlesTime[i]), handles[i].sizeDelta.y);
        }
    }

    public void InitPlayersDatas()
    {
        if (!GameManager.instance)
        {
            return;
        }

        lines.Clear();

        if (isLocal)
        {
            foreach (string player in GameManager.instance.profileNames)
            {
                Line newLine = new Line();

                LevelDatasStruct dat = new LevelDatasStruct();
                LevelDatas.LoadLevelDatas(GameManager.instance.levelToLoad, out dat, player);

                newLine.playerName = player;
                newLine.kaki = dat.coins;
                newLine.score = dat.score;
                newLine.stars = dat.stars;
                newLine.kills = dat.kills;
                newLine.medals = 0;

                lines.Add(newLine);
            }
        }
        else
        {
            foreach (string player in GameManager.instance.profileNames)
            {
                Line newLine = new Line();

                int medals = 0;
                int score = 0;
                int stars = 0;
                int kills = 0;
                int kaki = 0;

                int level = 0;
                LevelDatasStruct dat = new LevelDatasStruct();
                while (LevelDatas.LoadLevelDatas(level, out dat, player))
                {
                    score += dat.score;
                    stars += dat.stars;

                    level++;
                }

                if (PlayerPrefs.HasKey(player + "Collector"))
                {
                    medals += PlayerPrefs.GetInt(player + "Collector");
                }
                if (PlayerPrefs.HasKey(player + "Fisherman"))
                {
                    medals += PlayerPrefs.GetInt(player + "Fisherman");
                }
                if (PlayerPrefs.HasKey(player + "Darwinism"))
                {
                    medals += PlayerPrefs.GetInt(player + "Darwinism");
                }
                if (PlayerPrefs.HasKey(player + "Captain Cousteau"))
                {
                    medals += PlayerPrefs.GetInt(player + "Captain Cousteau");
                }
                if (PlayerPrefs.HasKey(player + "CoinPicked"))
                {
                    kaki = PlayerPrefs.GetInt(player + "CoinPicked");
                }
                if (PlayerPrefs.HasKey(player + "KillCount"))
                {
                    kills = PlayerPrefs.GetInt(player + "KillCount");
                }

                newLine.playerName = player;
                newLine.kaki = kaki;
                newLine.medals = medals;
                newLine.score = score;
                newLine.stars = stars;
                newLine.kills = kills;

                lines.Add(newLine);
            }
        }
    }

    private void OnEnable()
    {
        InitPlayersDatas();

        ShowLines(0);
        SetSelectedHandle(0);
    }

    public void ShowLines(int sortMode)
    {
        int childs = transform.childCount;
        for (int i = childs; i > 0; i--)
        {
            Destroy(transform.GetChild(i - 1).gameObject);
        }

        switch (sortMode)
        {
            case 0:
                lines.Sort((x, x2) => x.kaki.CompareTo(x2.kaki));
                lines.Reverse();

                int id = 1;
                foreach (Line line in lines)
                {
                    GameObject go = Instantiate(linePrefab, transform);

                    go.GetComponent<LeaderboardLine>().Setup(id, line.playerName, line.kaki);

                    id++;
                }

                break;
            case 1:
                lines.Sort((x, x2) => x.medals.CompareTo(x2.medals));
                lines.Reverse();

                int idd = 1;
                foreach (Line line in lines)
                {
                    GameObject go = Instantiate(linePrefab, transform);

                    go.GetComponent<LeaderboardLine>().Setup(idd, line.playerName, line.medals);

                    idd++;
                }

                break;
            case 2:
                lines.Sort((x, x2) => x.score.CompareTo(x2.score));
                lines.Reverse();

                int iddd = 1;
                foreach (Line line in lines)
                {
                    GameObject go = Instantiate(linePrefab, transform);

                    go.GetComponent<LeaderboardLine>().Setup(iddd, line.playerName, line.score);

                    iddd++;
                }

                break;
            case 3:
                lines.Sort((x, x2) => x.stars.CompareTo(x2.stars));
                lines.Reverse();

                int idddd = 1;
                foreach (Line line in lines)
                {
                    GameObject go = Instantiate(linePrefab, transform);

                    go.GetComponent<LeaderboardLine>().Setup(idddd, line.playerName, line.stars);

                    idddd++;
                }

                break;
            case 4:
                lines.Sort((x, x2) => x.kills.CompareTo(x2.kills));
                lines.Reverse();

                int iddddd = 1;
                foreach (Line line in lines)
                {
                    GameObject go = Instantiate(linePrefab, transform);

                    go.GetComponent<LeaderboardLine>().Setup(iddddd, line.playerName, line.kills);

                    iddddd++;
                }

                break;
        }

        AudioManager.Instance?.PlaySound("UIButton");
    }
}
