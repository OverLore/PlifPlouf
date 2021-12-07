using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    bool won;

    public bool canMobSpawn = true;
    public bool isEnded = false;

    public GameObject scoringCanvas;
    public Image scoringCanvasImg;

    public Sprite winSpr;
    public Sprite defSpr;

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
    [HideInInspector]
    public float maxLevelProgress = 60;

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

        GameManager.instance.LoseLife();
        won = false;
        canMobSpawn = true;
        isEnded = false;
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
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            state = LevelState.Waves;
        }
    }

    void UpdateWaves()
    {
        levelProgress += Time.deltaTime * GameManager.instance.timeScale;


        //if (levelProgress >= 15)
        //debug test level (level will be max 1 minute (even though we use percentage from 0 to 100))
        if (levelProgress >= maxLevelProgress)
        {
            KillAllEnemies();

            canMobSpawn = false;

            state = LevelState.BossStart;
        }
    }

    public void UpdateStarsBar()
    {
        if (updateStarsBar && won)
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

    public void KillAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach(Enemy e in enemies)
        {
            if (!e.isBoss)
            {
                e.ForceKill();
            }
        }
    }

    void UpdateScoring()
    {
        killsScore.Update();
        scoreScore.Update();
        coinsScore.Update();
        gainScore.Update();
        UpdateStarsBar();
    }

    
    public void StartScoring()
    {
        // get future coins
        int futureCoin = 0;
        // Attract seashells at the end of the level
        FindObjectsOfType<Coin>().ToList().ConvertAll(x => x.gameObject).ForEach(x => { StartCoroutine(Player.MoveTowardPlayer(x.transform.position, x, 0.5f));futureCoin++; });

        instance.isEnded = true;

        state = LevelState.Scoring;
        GameManager.instance.timeScale = 1;

        if (won)
        {
            scoringCanvasImg.sprite = winSpr;
        }
        else
        {
            scoringCanvasImg.sprite = defSpr;
        }

        scoringCanvas.SetActive(true);
        panelAnimator.SetTrigger("Pop");

        stars = Mathf.FloorToInt(GameManager.instance.Score / (maxObtainableScore * 1.2f) * 3f);
        stars = Mathf.Clamp(stars, 0, 3);

        GameManager.instance.ChangeMoney(coins + futureCoin);

        if (!won)
        {
            stars = 0;
        }

        killsScore.dest = kills;
        scoreScore.dest = GameManager.instance.Score;
        coinsScore.dest = coins + futureCoin;
        gainScore.dest = (float)kills + GameManager.instance.Score / 100 + coins;

        if (won)
        {
            LevelDatas.SaveLevelDatas(level, stars, (int)GameManager.instance.Score, kills, coins + futureCoin);

            GameManager.instance.lives++;
        }

        if (level+1 > GameManager.instance.maxLevelReached && won && level + 1 < 10)
        {
            GameManager.instance.ChangeMaxLevelReached(level+1);
        }

        PlayerPrefs.Save();
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
                if (level%2 == 1)
                {
                    BossEel.GetComponent<EelMove>().EelPhase = (level/2) % 5;
                    BossEel.GetComponent<Enemy>().PV = 100 + level % 5;
                    GameObject go = Instantiate(BossEel);

                    state = LevelState.BossPlay;
                }
                else
                {
                    state = LevelState.BossEnd;
                }

                break;
            case LevelState.BossEnd:
                if (GameManager.instance.GetPlayer().pv <= 0)
                {
                    return;
                }

                won = true;

                StartScoring();

                break;
            case LevelState.Scoring:
                UpdateScoring();

                break;
        }
    }
}
