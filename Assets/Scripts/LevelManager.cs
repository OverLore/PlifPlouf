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
    Scoring
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int level;

    LevelState state;

    public float levelProgress;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        levelProgress += Time.deltaTime;
    }
}
