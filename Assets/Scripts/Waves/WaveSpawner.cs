using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] string levelToLoad;

    GameObject fishPrefab;

    Wave wave;
    int passedWaves = 0;

    float lastProgress = 0f;

    private void Start()
    {
        fishPrefab = Resources.Load<GameObject>("Prefabs/Fish");
    }

    public void LoadLevelWaves(string level)
    {
        levelToLoad = level;

        string path = $"Levels/{levelToLoad}";

        TextAsset jsonTextFile = Resources.Load<TextAsset>(path);

        wave = (Wave)JsonUtility.FromJson(jsonTextFile.text, typeof(Wave));
    }

    private void Update()
    {
        if (wave == null || passedWaves >= wave.percentages.Count)
        {
            return;
        }

        if (lastProgress <= wave.percentages[passedWaves] &&
            LevelManager.instance.levelProgress > wave.percentages[passedWaves])
        {
            SpawnWave();

            passedWaves++;
        }

        lastProgress = LevelManager.instance.levelProgress;
    }

    void CreateEnemy(Vector3 offset, GameObject group)
    {
        GameObject g = Instantiate(group);
        g.transform.position = transform.position + offset;
    }

    void SpawnWave()
    {
        List<WaveSequence> sequence = new List<WaveSequence>();

        string path = $"Waves/{wave.sequences[passedWaves]}/";

        TextAsset[] jsonTextFile = Resources.LoadAll<TextAsset>(path);

        sequence.Clear();

        foreach (TextAsset txt in jsonTextFile)
        {
            sequence.Add((WaveSequence)JsonUtility.FromJson(txt.text, typeof(WaveSequence)));
        }

        float delay = 0;

        for (int i = 0; i < sequence.Count; i++)
        {
            delay += sequence[i].time;

            StartCoroutine(SpawnSeq(delay, sequence[i].Offset, sequence[i].path));
        }
    }

    IEnumerator SpawnSeq(float delay, List<Vector2> offset, List<string> paths)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < paths.Count; i++)
        {
            GameObject go = Resources.Load<GameObject>(paths[i]);

            //make cleaner method
            if (go.name == "E4Pattern1L" ||
                go.name == "E4Pattern1R")
            {
                Vector3 enemyPos = go.transform.GetChild(0).GetChild(0).Find("E4Test").position;
                //Vector3 routePos = go.transform.position -  go.transform.GetChild(0).GetChild(0).position;
                //DangerSignManager.instance.SpawnDangerSign(new Vector2(enemyPos.x, routePos.y));
                DangerSignManager.instance.SpawnDangerSign(enemyPos);
            }

            CreateEnemy(offset[i], go);
        }
    }
}
