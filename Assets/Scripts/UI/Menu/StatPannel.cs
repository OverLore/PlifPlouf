using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPannel : MonoBehaviour
{
    [SerializeField] Text Level;
    [SerializeField] Text Stars;
    [SerializeField] Text Score;
    [SerializeField] Text Kills;
    [SerializeField] Text Coins;
    LevelDatasStruct DataLevelTest;

    // Update is called once per frame
    void Update()
    {
        Level.text = "Level " + (GameManager.instance.levelToLoad +1);
        LevelDatas.LoadLevelDatas(GameManager.instance.levelToLoad, out DataLevelTest);
        //if (LevelDatas.LoadLevelDatas(1, out DataLevelTest) == true)
        //{
        //
        //}
       Stars.text = "Stars " + (DataLevelTest.stars);
        Score.text = "Score " + (DataLevelTest.score);
        Kills.text = "Kills " + (DataLevelTest.kills);
        Coins.text = "Coins " + (DataLevelTest.coins);
    }
}
