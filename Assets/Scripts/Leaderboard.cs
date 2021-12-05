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
    }

    [SerializeField] GameObject linePrefab;

    List<Line> lines = new List<Line>();

    public void InitPlayersDatas()
    {
        if (!GameManager.instance)
        {
            return;
        }

        lines.Clear();

        foreach (string player in GameManager.instance.profileNames)
        {
            Line newLine = new Line();

            int medals = 0;
            int score = 0;
            int stars = 0;
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

            newLine.playerName = player;
            newLine.kaki = kaki;
            newLine.medals = medals;
            newLine.score = score;
            newLine.stars = stars;

            lines.Add(newLine);
        }
    }

    private void OnEnable()
    {
        InitPlayersDatas();

        ShowLines(0);
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
        }
    }
}
