using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float scrollingSpeed;
    public int maxDepthLayer = 10;

    public static GameManager instance;
    public WaveSpawner waveSpawner;

    public float levelProgress;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        levelProgress = 0f;

        waveSpawner.LoadLevelWaves();
    }

    // Update is called once per frame
    void Update()
    {
        levelProgress += Time.deltaTime;
    }
}
