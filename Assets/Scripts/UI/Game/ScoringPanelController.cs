using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringPanelController : MonoBehaviour
{
    public ParticleSystem coinsVFX;

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

    public void UpdateStars()
    {
        LevelManager.instance.updateStarsBar = true;
    }

    public void SetupCoinFX()
    {
        var burst = coinsVFX.emission.GetBurst(0);

        int i = Mathf.FloorToInt(LevelManager.instance.gainScore.dest / 5);

        if (LevelManager.instance.gainScore.dest > 0)
        {
            i++;
        }

        burst.count = i;

        coinsVFX.emission.SetBurst(0, burst);
    }

    public void SpawnCoinVFX()
    {
        coinsVFX.Play();

        if (LevelManager.instance.gainScore.dest > 0)
        {
            AudioManager.Instance.PlaySound("Moulaga");
        }
    }
}
