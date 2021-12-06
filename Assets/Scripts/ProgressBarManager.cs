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
    RectTransform dotRT;
    Image barFullImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        dotRT = progressBarDot.GetComponent<RectTransform>();
        barFullImage = progressBarFull.GetComponent<Image>();
    }

    void Update()
    {
        float fillAmount = LevelManager.instance.levelProgress / LevelManager.instance.maxLevelProgress;
        Vector3 pos = dotRT.position;
        pos.y = Mathf.Lerp(progressMinRT.position.y, progressMaxRT.position.y, fillAmount);
        barFullImage.fillAmount = fillAmount;
        Debug.Log(fillAmount);


        dotRT.position = pos;
    }
}
