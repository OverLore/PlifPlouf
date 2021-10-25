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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        Application.targetFrameRate = 60;
    }

    public void LoadLevel()
    {
        LevelManager.instance.StartLevel(levelToLoad);

        waveSpawner.LoadLevelWaves();
    }
}
