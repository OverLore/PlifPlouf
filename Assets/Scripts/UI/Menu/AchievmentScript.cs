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
                    collectorSetDesc(tabAchievment[i].ReachedRank, i);
                    break;
                case "Fisherman":
                    FishermanSetDesc(tabAchievment[i].ReachedRank, i);
                    break;
                case "Darwinism":
                    DarwinismSetDesc(tabAchievment[i].ReachedRank, i);
                    break;
            }


        }
        SaveAchievments();
    }
    void collectorSetDesc(int _reachedRank, int _i)
    {
        if (_reachedRank > 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "collect 250 kakillages";
        }
        if (_reachedRank > 1)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "collect 500 kakillages";
        }
        if (_reachedRank > 2)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Completed!";
        }
    }
    void FishermanSetDesc(int _reachedRank, int _i)
    {
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
    void DarwinismSetDesc(int _reachedRank, int _i)
    {
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
    void CousteauSetDesc(int _reachedRank, int _i)
    {
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