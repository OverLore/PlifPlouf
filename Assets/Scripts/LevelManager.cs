using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum LevelState
{
    Starting,
    Waves,
    BossStart,
    BossPlay,
    BossEnd,
    Scoring,
    None
}

public class LevelManager : MonoBehaviour
{
    public Text debugText;

    public static LevelManager instance;
    public int level;

    LevelState state = LevelState.None;

    public float levelProgress;

    public void StartLevel(int _level)
    {
        level = _level;

        state = LevelState.Starting;
    }

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Awake()
    {
        Init();
    }

    void UpdateStarting()
    {
        debugText.text = "Press screen to start";

        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            state = LevelState.Waves;
        }
    }

    void UpdateWaves()
    {
        levelProgress += Time.deltaTime;
        debugText.text = levelProgress.ToString();

        if (levelProgress >= 100)
        {
            state = LevelState.BossStart;
        }
    }

    void UpdateScoring()
    {

    }

    void Update()
    {
        switch(state)
        {
            case LevelState.None:
                GameManager.instance.LoadLevel();
                return;
            case LevelState.Starting:
                UpdateStarting();

                break;
            case LevelState.Waves:
                UpdateWaves();

                break;
            case LevelState.BossEnd:
                state = LevelState.Scoring;
                Time.timeScale = 1;

                break;
            case LevelState.Scoring:
                UpdateScoring();

                break;
        }
    }
}
