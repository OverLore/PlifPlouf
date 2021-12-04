using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchievmentScript : MonoBehaviour
{
    public Achievment[] tabAchievment = new Achievment[7];


    void Start()
    {
        LoadAchievments();
    }

    void Update()
    {
        //checking of achievment conditions


        //end of checking 

        for (int i = 0; i < tabAchievment.Length; i++)
        {
            if (tabAchievment[i].ReachedRank > 0)
            {
                tabAchievment[i].AchievmentPannel.gameObject.transform.Find("RankOneOn").gameObject.SetActive(true);
            }
            if (tabAchievment[i].ReachedRank > 1)
            {
                tabAchievment[i].AchievmentPannel.gameObject.transform.Find("RankTwoOn").gameObject.SetActive(true);
            }
            if (tabAchievment[i].ReachedRank > 2)
            {
                tabAchievment[i].AchievmentPannel.gameObject.transform.Find("RankThreeOn").gameObject.SetActive(true);
            }

            switch (tabAchievment[i].name)
            {
                case "Collector":
                    collectorSetStat(tabAchievment[i].ReachedRank, i);
                    break;
                case "Fisherman":
                    FishermanSetStat(tabAchievment[i].ReachedRank, i);
                    break;
                case "Darwinism":
                    DarwinismSetStat(tabAchievment[i].ReachedRank, i);
                    break;
                case "Captain Cousteau":
                    CousteauSetStat(tabAchievment[i].ReachedRank, i);
                    break;
            }


        }
        SaveAchievments();
    }
    void collectorSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank == 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "collect 100 kakillages";
            if(PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") >= 100 && PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") < 250)
            {
                //PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i].name, 1);
                tabAchievment[_i].ReachedRank = 1;
            }
        }
        if (_reachedRank == 1)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "collect 250 kakillages";
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") >= 250 && PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") < 500)
            {
                //PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i].name, 2);
                tabAchievment[_i].ReachedRank = 2;
            }
        }
        if (_reachedRank == 2)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "collect 500 kakillages";
            if (PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") >= 500)
            {
               // PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[_i].name, 3);
                tabAchievment[_i].ReachedRank = 3;
            }
        }
        if (_reachedRank == 3)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Completed!";
        }
    }
    void FishermanSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank == 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Beat 25 ennemies";
        }
        if (_reachedRank > 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Beat 50 ennemies";
        }
        if (_reachedRank > 1)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Beat 100 ennemies";
        }
        if (_reachedRank > 2)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Completed!";
        }
    }
    void DarwinismSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank == 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Upgrade yourself 5 times";
        }
        if (_reachedRank > 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Upgrade yourself 10 times";
        }
        if (_reachedRank > 1)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Upgrade yourself 20 times";
        }
        if (_reachedRank > 2)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Completed!";
        }
    }
    void CousteauSetStat(int _reachedRank, int _i)
    {
        if (_reachedRank == 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Reach level 5";
        }
        if (_reachedRank > 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Reach level 10";
        }
        if (_reachedRank > 1)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Reach level 15";
        }
        if (_reachedRank > 2)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Completed!";
        }
    }
    public void LoadAchievments()
    {
        for (int i = 0; i < tabAchievment.Length; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.instance.profileName + tabAchievment[i].name))
            {
                tabAchievment[i].ReachedRank = PlayerPrefs.GetInt(GameManager.instance.profileName + tabAchievment[i].name);
            }
            else
            {
                tabAchievment[i].ReachedRank = 0;
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[i].name, 0);
            }
        }
    }
    public void SaveAchievments()
    {
        for (int i = 0; i < tabAchievment.Length; i++)
        {
            
                PlayerPrefs.SetInt(GameManager.instance.profileName + tabAchievment[i].name, tabAchievment[i].ReachedRank);
            
        }
    }
}



[System.Serializable]
public class Achievment
{
    public string name;
    [SerializeField] public GameObject AchievmentPannel;
    public int ReachedRank;



}