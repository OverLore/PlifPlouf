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

    public int levelToLoad;

    // score
    [field: SerializeField] private ulong score;
    public ulong Score { get => score; }
    [SerializeField] private uint scoreAdded = 10;

    // combo
    [Header("Combo")]
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

    public int maxLevelReached = 0;

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
            maxLevelReached = 0;
            PlayerPrefs.SetInt("maxLevelReached", 0);
        }

        Application.targetFrameRate = 60;

        GetTexts();
    }

    private void Update()
    {
        UpdateCombo();
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
            comboTimer -= Time.deltaTime;
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
