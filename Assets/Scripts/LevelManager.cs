using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static LevelManager instance;
    public int level;

    LevelState state = LevelState.None;

    public float levelProgress;

    public void StartLevel(int _level)
    {
        level = _level;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (state == LevelState.None)
        {
            GameManager.instance.LoadLevel();
        }

        levelProgress += Time.deltaTime;
    }
}
