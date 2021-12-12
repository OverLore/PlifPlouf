using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotfixUwu : MonoBehaviour
{
    public string[] tabAchievment;

    void Start()
    {
        LoadAchievments();

        for (int i = 0; i < tabAchievment.Length; i++)
        {
            switch (tabAchievment[i])
            {
                case "Collector":
                    collectorSetStat(PlayerPrefs.GetInt(GameManager.instance.profileName + tabAchievment[i]), i);
                    break;
                case "Fisherman":
                    FishermanSetStat(PlayerPrefs.GetInt(GameManager.instance.profileName + tabAchievment[i]), i);
                    break;
                case "Darwinism":
                    DarwinismSetStat(PlayerPrefs.GetInt(GameManager.instance.profileName + tabAchievment[i]), i);
                    break;
                case "Captain Cousteau":
                    CousteauSetStat(PlayerPrefs.GetInt(GameManager.instance.profileName + tabAchievment[i]), i);
                    break;
            }
        }
    }

    void collectorSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank < 1)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") >= 100 && PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") < 250)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 1);
            }
        }
        else if (_reachedRank < 2)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") >= 250 && PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") < 500)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 2);
            }
        }
        else if (_reachedRank < 3)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") >= 500)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 3);
            }
        }
    }
    void FishermanSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank < 1)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "KillCount") >= 25 && PlayerPrefs.GetInt(GameManager.instance.profileName + "KillCount") < 50)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 1);
            }
        }
        else if (_reachedRank < 2)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "KillCount") >= 50 && PlayerPrefs.GetInt(GameManager.instance.profileName + "KillCount") < 100)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 2);
            }
        }
        else if (_reachedRank < 3)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "KillCount") >= 100)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 3);
            }
        }
    }
    void DarwinismSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank < 1)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "UpgradeCount") >= 5 && PlayerPrefs.GetInt(GameManager.instance.profileName + "UpgradeCount") < 10)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 1);
            }
        }
        else if (_reachedRank < 2)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "UpgradeCount") >= 10 && PlayerPrefs.GetInt(GameManager.instance.profileName + "UpgradeCount") < 20)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 2);
            }
        }
        else if (_reachedRank < 3)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "UpgradeCount") >= 20)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 3);
            }
        }
    }
    void CousteauSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank < 1)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached") >= 4 && PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached") < 9)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 1);
            }
        }
        else if (_reachedRank < 2)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached") >= 9 && PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached") < 14)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 2);
            }
        }
        else if (_reachedRank < 3)
        {
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached") >= 14)
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i], 3);
            }
        }
    }

    public void LoadAchievments()
    {
        for (int i = 0; i < tabAchievment.Length; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.instance.profileName + tabAchievment[i]))
            {
                //do nothing
            }
            else
            {
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[i], 0);
            }
        }
    }
}
