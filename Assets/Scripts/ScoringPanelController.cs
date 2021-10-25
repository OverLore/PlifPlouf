using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringPanelController : MonoBehaviour
{
    public void UpdateKills()
    {
        LevelManager.instance.killsScore.updating = true;
    }

    public void UpdateScores()
    {
        LevelManager.instance.scoreScore.updating = true;
    }

    public void UpdateCoins()
    {
        LevelManager.instance.coinsScore.updating = true;
    }

    public void UpdateGain()
    {
        LevelManager.instance.gainScore.updating = true;
    }
}
