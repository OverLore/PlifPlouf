using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchievmentScript : MonoBehaviour
{
    public Achievment[] tabAchievment = new Achievment[7];


    void Start()
    {

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
            }


        }
    }
    void collectorSetDesc(int _reachedRank, int _i)
    {
        if (_reachedRank > 0)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Have more than 2500 kakillages";
        }
        if (_reachedRank > 1)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Have more than 5000 kakillages";
        }
        if (_reachedRank > 2)
        {
            tabAchievment[_i].AchievmentPannel.gameObject.transform.Find("Desc").gameObject.GetComponent<Text>().text = "Completed!";
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