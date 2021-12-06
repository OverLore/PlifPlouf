using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    private static ProgressBarManager instance;
    public static ProgressBarManager Instance { get { return instance; } }
    [SerializeField] GameObject progressBarDot;
    [SerializeField] GameObject progressBarFull;
    [SerializeField] RectTransform progressMinRT;
    [SerializeField] RectTransform progressMaxRT;
    RectTransform dotRT;

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
    }

    void Update()
    {
        Vector3 pos = dotRT.position;
        pos.y = Mathf.Lerp(progressMinRT.position.y, progressMaxRT.position.y, LevelManager.instance.levelProgress / LevelManager.instance.maxLevelProgress);
        Debug.Log(LevelManager.instance.levelProgress / LevelManager.instance.maxLevelProgress);

        dotRT.position = pos;
    }
}
