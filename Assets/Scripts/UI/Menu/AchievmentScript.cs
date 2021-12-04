using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentScript : MonoBehaviour
{
    public Achievment[] tabAchievment = new Achievment[7];


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

[System.Serializable]
public class Achievment
{
    public string name;
    [SerializeField] GameObject AchievmentPannel;
    public int ReachedRank;



}