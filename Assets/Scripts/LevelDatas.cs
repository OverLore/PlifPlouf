using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelDatasStruct
{
    public int stars;
    public int score;
    public int kills;
    public int coins;

    public LevelDatasStruct(int _stars, int _score, int _kills, int _coins)
    {
        stars = _stars;
        score = _score;
        kills = _kills;
        coins = _coins;
    }
}

public class LevelDatas : MonoBehaviour
{
    public static void SaveLevelDatas(int level, int stars, int score, int kills, int coins)
    {
        LevelDatasStruct dats;

        if (LoadLevelDatas(level, out dats))
        {
            if (dats.score < score)
            {
                PlayerPrefs.SetInt($"{level}_stars", stars);
                PlayerPrefs.SetInt($"{level}_score", score);
                PlayerPrefs.SetInt($"{level}_kills", kills);
                PlayerPrefs.SetInt($"{level}_coins", coins);
            }
        }
        else
        {
            PlayerPrefs.SetInt($"{level}_stars", stars);
            PlayerPrefs.SetInt($"{level}_score", score);
            PlayerPrefs.SetInt($"{level}_kills", kills);
            PlayerPrefs.SetInt($"{level}_coins", coins);
        }
    }

    public static bool LoadLevelDatas(int level, out LevelDatasStruct dats)
    {
        if (PlayerPrefs.HasKey($"{level}_stars"))
        {
            dats.stars = PlayerPrefs.GetInt($"{level}_stars");
            dats.score = PlayerPrefs.GetInt($"{level}_score");
            dats.kills = PlayerPrefs.GetInt($"{level}_kills");
            dats.coins = PlayerPrefs.GetInt($"{level}_coins");
            return true;
        }

        dats.stars = 0;
        dats.score = 0;
        dats.kills = 0;
        dats.coins = 0;
        return false;
    }
}
