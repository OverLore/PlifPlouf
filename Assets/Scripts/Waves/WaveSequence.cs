using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSequence
{
    public int amount = 0;
    public float time = 0;

    public List<GameObject> Group = new List<GameObject>();
    public List<string> path = new List<string>();
    public List<Vector2> Offset = new List<Vector2>();
}

[System.Serializable]
public class Wave
{
    public int amount = 0;

    public List<string> sequences = new List<string>();
    public List<float> percentages = new List<float>();
    public List<Vector2> offsets = new List<Vector2>();
}
