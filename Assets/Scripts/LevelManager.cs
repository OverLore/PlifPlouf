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
    [System.Serializable]
    public struct AnimatedScore
    {
        public bool updating;
        public Text text;
        float temp;
        float act;
        public float dest;

        public void InitText()
        {
            text.text = string.Empty;
        }

        public void Update()
        {
            if (updating)
            {
                temp += Time.deltaTime;

                temp = Mathf.Clamp01(temp);

                act = Mathf.Lerp(0, dest, temp);
                text.text = ((int)act).ToString();
            }
        }
    }

    public Text debugText;

    public GameObject scoringCanvas;
    public Animator panelAnimator;

    public static LevelManager instance;
    public int level;

    LevelState state = LevelState.None;

    public float levelProgress;

    public float score;
    public int kills;
    public int coins;

    public AnimatedScore killsScore;
    public AnimatedScore scoreScore;
    public AnimatedScore coinsScore;
    public AnimatedScore gainScore;

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

        killsScore.InitText();
        scoreScore.InitText();
        coinsScore.InitText();
        gainScore.InitText();

        scoringCanvas.SetActive(false);
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

        if (levelProgress >= 10)
        {
            state = LevelState.BossEnd;
        }
    }

    public void UpdateKills()
    {
        killsScore.updating = true;
    }

    public void UpdateScores()
    {
        scoreScore.updating = true;
    }

    public void UpdateCoins()
    {
        coinsScore.updating = true;
    }

    public void UpdateGain()
    {
        gainScore.updating = true;
    }

    void UpdateScoring()
    {
        killsScore.Update();
        scoreScore.Update();
        coinsScore.Update();
        gainScore.Update();
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

                scoringCanvas.SetActive(true);
                panelAnimator.SetTrigger("Pop");

                killsScore.dest = kills;
                scoreScore.dest = score;
                coinsScore.dest = coins;
                gainScore.dest = kills + (int)(score / 100) + coins;

                if (level > GameManager.instance.maxLevelReached)
                {
                    GameManager.instance.ChangeMaxLevelReached(level);
                }

                break;
            case LevelState.Scoring:
                UpdateScoring();

                break;
        }
    }
}
