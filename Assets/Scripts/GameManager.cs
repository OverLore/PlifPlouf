using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float scrollingSpeed;
    public int maxDepthLayer = 10;

    public static GameManager instance;
    public WaveSpawner waveSpawner;

    public int levelToLoad;

    public int maxLevelReached;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        if (PlayerPrefs.HasKey("maxLevelReached"))
        {
            maxLevelReached = PlayerPrefs.GetInt("maxLevelReached");
        }
        else
        {
            maxLevelReached = 0;
            PlayerPrefs.SetInt("maxLevelReached", 0);
        }

        Application.targetFrameRate = 60;
    }

    public void ChangeMaxLevelReached(int max)
    {
        maxLevelReached = max;
        PlayerPrefs.SetInt("maxLevelReached", maxLevelReached);
    }

    public void LoadLevel()
    {
        LevelManager.instance.StartLevel(levelToLoad);

        waveSpawner.LoadLevelWaves();
    }
}
