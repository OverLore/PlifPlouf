using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    private static ProgressBarManager instance;
    public static ProgressBarManager Instance { get { return instance; } }
    [SerializeField] GameObject progressBarDot;
    [SerializeField] GameObject progressBarFull;
    [SerializeField] RectTransform progressMinRT;
    [SerializeField] RectTransform progressMaxRT;
    [SerializeField] Canvas progressBarCanvas;
    RectTransform dotRT;
    Image barFullImage;
    CanvasGroup canvasGroup;
    [HideInInspector] public static float alphaTimer;

    void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(instance);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    void Start()
    {
        dotRT = progressBarDot.GetComponent<RectTransform>();
        barFullImage = progressBarFull.GetComponent<Image>();
        canvasGroup = progressBarCanvas.GetComponent<CanvasGroup>();

        alphaTimer = 1.0f;
    }

    void Update()
    {
        float fillAmount = LevelManager.instance.levelProgress / LevelManager.instance.maxLevelProgress;
        Vector3 pos = dotRT.position;
        pos.y = Mathf.Lerp(progressMinRT.position.y, progressMaxRT.position.y, fillAmount);
        barFullImage.fillAmount = fillAmount;
        //Debug.Log(fillAmount);

        //alphaTimer incremented in GameManager
        alphaTimer = Mathf.Clamp01(alphaTimer);
        canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, alphaTimer);

        dotRT.position = pos;
    }
}
