using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float scrollingSpeed;
    public int maxDepthLayer = 10;

    public static GameManager instance;
    public WaveSpawner waveSpawner;

    [SerializeField] Image pauseBack;

    public bool Paused;

    public int levelToLoad;

    public float timeScale = 1f;

    [Space(10), Header("Stats")]
    public int money = 2000;
    public int lives = 5;

    // score
    [field: SerializeField] private ulong score;
    public ulong Score { get => score; }
    [SerializeField] private uint scoreAdded = 10;

    public Player player;

    // combo
    [Space(10), Header("Combo")]
    [SerializeField] private double comboFactor = 1.0;
    private float comboTimer = 0.0f;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float comboTime = 2.5f;
    [SerializeField]
    [Range(5.0f, 10.0f)]
    float comboAngleShake = 7.5f;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float comboFactToMaxScale = 5f;
    [SerializeField]
    [Range(1.0f, 3.0f)]
    float comboTextMaxScale = 2f;

    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI comboText = null;
    Animator comboAnimator;

    public int maxLevelReached = -1;

    void LoadStats()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            money = 2000;
            PlayerPrefs.SetInt("Money", money);
        }

        if (PlayerPrefs.HasKey("Lives"))
        {
            lives = PlayerPrefs.GetInt("Lives");
        }
        else
        {
            lives = 5;
            PlayerPrefs.SetInt("Lives", lives);
        }
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Lives", lives);
    }

    public void ChangeMoney(int _modifier)
    {
        money += _modifier;
        money = Mathf.Clamp(money, 0, 999999);

        PlayerPrefs.SetInt("Money", money);
    }

    public void ChangeLives(int _modifier)
    {
        lives += _modifier;
        lives = Mathf.Clamp(lives, 0, 5);

        PlayerPrefs.SetInt("Lives", lives);
    }

    private void GetTexts()
    {
        var textObjects = FindObjectsOfType<TextMeshProUGUI>();
        foreach (var textObject in textObjects)
        {
            if (textObject.name == "Score")
            {
                scoreText = textObject;
            }
            if (textObject.name == "Combo")
            {
                comboText = textObject;
                comboAnimator = comboText.GetComponent<Animator>();
            }
        }
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("maxLevelReached"))
        {
            maxLevelReached = PlayerPrefs.GetInt("maxLevelReached");
        }
        else
        {
            maxLevelReached = -1;
            PlayerPrefs.SetInt("maxLevelReached", -1);
        }

        Application.targetFrameRate = 60;

        GetTexts();

        LoadStats();

        //use it like that only to initialize the player reference
        HasPlayer();
    }

    private void Update()
    {
        UpdateCombo();

        if (Paused)
        {
            timeScale = Mathf.Lerp(timeScale, .05f, Time.deltaTime * 5f);
            pauseBack.color = Color.Lerp(pauseBack.color, new Color(0, 0, 0, 185 / 255), Time.deltaTime * 5f);
        }
        else
        {
            timeScale = Mathf.Lerp(timeScale, 1f, Time.deltaTime * 5f);
            pauseBack.color = Color.Lerp(pauseBack.color, new Color(0, 0, 0, 0), Time.deltaTime * 5f);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused = !Paused;
        }
    }

    public void ChangeMaxLevelReached(int max)
    {
        maxLevelReached = max;
        PlayerPrefs.SetInt("maxLevelReached", maxLevelReached);
    }

    public void LoadLevel()
    {
        LevelManager.instance.StartLevel(levelToLoad);

        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        waveSpawner.LoadLevelWaves(levelToLoad.ToString());
    }

    bool HasPlayer()
    {
        if (player == null)
        {
            player = GameObject.Find("Player")?.GetComponent<Player>();
        }

        return player != null;
    }

    public void ActivateAttackSpeed()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateAttackSpeed();
        }
    }

    public void ActivateAttackDamage()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateAttackDamage();
        }
    }

    public void ActivateShiel()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateShiel();
        }
    }

    public void ActivateHorizontalShot()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateHorizontalShot();
        }
    }

    public void ActivateShotNumber()
    {
        if (instance.HasPlayer())
        {
            instance.player.ActivateShotNumber();
        }
    }

    #region SCORE

    public void AddScore()
    {
        IncreaseCombo();
        score += (ulong)((double)scoreAdded * comboFactor);
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    public void AddScore(uint _score)
    {
        IncreaseCombo();
        score += (ulong)((double)_score * comboFactor);
        if (scoreText != null)
        {
            scoreText.text = $"SCORE : {score}";
        }
    }

    public void ResetScore()
    {
        score = 0;
    }

    #endregion

    #region COMBO

    private void UpdateCombo()
    {
        if (comboText == null)
        {
            return;
        }

        if (comboFactor > 1f)
        {
            comboTimer -= Time.deltaTime * instance.timeScale;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
            comboText.enabled = true;
        }
        else
        {
            comboText.enabled = false;
        }
    }

    private void IncreaseCombo()
    {
        comboFactor += 0.05;

        string str;
        if (comboFactor < 10)
        {
            str = $"Combo x{comboFactor.ToString("0.00")} !";
        }
        else if (comboFactor < 100)
        {
            str = $"Combo x{comboFactor.ToString("00.0")} !";
        }
        else if (comboFactor < 1000)
        {
            str = $"Combo x{comboFactor.ToString("000")} !";
        }
        else
        {
            str = $"Combo xtreme !";
        }
        //string str = $"Combo x{comboFactor.ToString("0.###E+00")} !";
        //for(int i = 0; i < comboFactor - 2;i++) {str += "!";}
        comboText.text = str;

        comboAnimator.Play("Combo",-1,0);


        comboText.transform.rotation = 
            Quaternion.Euler(
                new Vector3(0, 0,
                Random.Range(-comboAngleShake, comboAngleShake)));

        float scale = Mathf.Lerp(50, 60, 
            Mathf.Clamp((float)comboFactor, 1, comboFactToMaxScale) / comboFactToMaxScale);
        comboText.fontSize = (int)scale;
        //comboText.transform.localScale = new Vector3(scale, scale, 1);
        comboText.transform.localPosition = new Vector3(0f,-120f -10f * 
            Mathf.Clamp((float)comboFactor, 1, comboFactToMaxScale)
            , 0f);


        comboTimer = comboTime;
    }

    private void ResetCombo()
    {
        comboFactor = 1.0;
        comboTimer = comboTime;
    }
    #endregion

    private void OnLevelWasLoaded(int level)
    {
        GetTexts();
    }
}
