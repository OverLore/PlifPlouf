using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LevelState
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

    public GameObject CoinPrefab;

    public GameObject BossEel;

    public ParticleSystem[] starsParticles;
    public Image[] starsSprites;
    public Image starBar;
    float starsBarTemp = 0;
    float starsBarAnim = 0;
    int lastStarsBarAnim = -1;
    public bool updateStarsBar;

    public static LevelManager instance;
    public int level;

    public LevelState state = LevelState.None;

    public float levelProgress;

    public int maxObtainableScore = 0;

    //public int score;
    public int kills = 0;
    public int coins = 0;
    public int stars = 0;

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

        CoinPrefab = Resources.Load<GameObject>("Entities/Coin");
        BossEel = Resources.Load<GameObject>("Prefabs/Eel");
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
        levelProgress += Time.deltaTime * GameManager.instance.timeScale;
        debugText.text = levelProgress.ToString();

        
        //if (levelProgress >= 15)
        //debug test level (level will be max 1 minute (even though we use percentage from 0 to 100))
        if (levelProgress >= 61)
        {
            state = LevelState.BossStart;
        }
    }

    public void UpdateStarsBar()
    {
        if (updateStarsBar)
        {
            int i = 0;

            starsBarTemp += Time.deltaTime;
            starsBarTemp = Mathf.Clamp01(starsBarTemp);

            starsBarAnim = Mathf.Lerp(0, GameManager.instance.Score, starsBarTemp);

            i = Mathf.FloorToInt(starsBarAnim / (maxObtainableScore * 1.2f) * 3f);
            i = Mathf.Clamp(i, 0, 3);

            if (maxObtainableScore == 0)
            {
                starBar.fillAmount = 0;
            }
            else
            {
                starBar.fillAmount = starsBarAnim / (maxObtainableScore * 1.2f);
                starBar.fillAmount = Mathf.Clamp01(starBar.fillAmount);
            }

            if (lastStarsBarAnim != i && i != 0)
            {
                starsSprites[i - 1].color = new Color(1, 1, 1, 1);
                starsParticles[i - 1].Play();
            }

            lastStarsBarAnim = i;
        }
    }

    public void SpawnCoinAt(Vector3 _pos,  int _value)
    {
        GameObject go = Instantiate(CoinPrefab);
        go.transform.position = _pos;

        go.GetComponent<Coin>().Setup(_value);
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
        UpdateStarsBar();
    }

    public void StartScoring(bool won)
    {
        state = LevelState.Scoring;
        GameManager.instance.timeScale = 1;

        scoringCanvas.SetActive(true);
        panelAnimator.SetTrigger("Pop");

        stars = Mathf.FloorToInt(GameManager.instance.Score / (maxObtainableScore * 1.2f) * 3f);
        stars = Mathf.Clamp(stars, 0, 3);

        if (!won)
        {
            stars = 0;
        }

        killsScore.dest = kills;
        scoreScore.dest = GameManager.instance.Score;
        coinsScore.dest = coins;
        gainScore.dest = (float)kills + GameManager.instance.Score / 100 + coins;

        if (won)
        {
            LevelDatas.SaveLevelDatas(level, stars, (int)GameManager.instance.Score, kills, coins);
        }

        if (level > GameManager.instance.maxLevelReached && won)
        {
            GameManager.instance.ChangeMaxLevelReached(level);
        }
    }

    void Update()
    {
        switch(state)
        {
            case LevelState.None:
                GameManager.instance.LoadLevel();
                return;
            case LevelState.Starting:
                if (GameManager.instance.GetPlayer().pv <= 0)
                {
                    return;
                }

                UpdateStarting();

                break;
            case LevelState.Waves:
                if (GameManager.instance.GetPlayer().pv <= 0)
                {
                    return;
                }

                UpdateWaves();

                break;
            case LevelState.BossStart:
                if (GameManager.instance.GetPlayer().pv <= 0)
                {
                    return;
                }
                if (level % 10 <= 5)
                {
                    GameObject go = Instantiate(BossEel);
                }
                else
                {
                    //todo second boss          
                }

                state = LevelState.BossPlay;

                break;
            case LevelState.BossEnd:
                if (GameManager.instance.GetPlayer().pv <= 0)
                {
                    return;
                }

                StartScoring(true);

                break;
            case LevelState.Scoring:
                UpdateScoring();

                break;
        }
    }
}
