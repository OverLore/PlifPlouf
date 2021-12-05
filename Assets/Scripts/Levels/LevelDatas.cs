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
                PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_stars", stars);
                PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_score", score);
                PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_kills", kills);
                PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_coins", coins);
            }
        }
        else
        {
            PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_stars", stars);
            PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_score", score);
            PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_kills", kills);
            PlayerPrefs.SetInt($"{GameManager.instance.profileName}{level}_coins", coins);
        }
    }

    public static bool LoadLevelDatas(int level, out LevelDatasStruct dats)
    {
        if (PlayerPrefs.HasKey($"{GameManager.instance.profileName}{level}_stars"))
        {
            dats.stars = PlayerPrefs.GetInt($"{GameManager.instance.profileName}{level}_stars");
            dats.score = PlayerPrefs.GetInt($"{GameManager.instance.profileName}{level}_score");
            dats.kills = PlayerPrefs.GetInt($"{GameManager.instance.profileName}{level}_kills");
            dats.coins = PlayerPrefs.GetInt($"{GameManager.instance.profileName}{level}_coins");
            return true;
        }

        dats.stars = 0;
        dats.score = 0;
        dats.kills = 0;
        dats.coins = 0;
        return false;
    }
}
