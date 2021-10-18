using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    readonly string waveRootPath = "Waves/";

    public void LoadLevelWaves(int levelID)
    {

    }
}
