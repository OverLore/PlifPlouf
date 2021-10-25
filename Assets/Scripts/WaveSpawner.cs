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

    public void LoadLevelWaves()
    {
        string path = $"Levels/{levelToLoad}";

        TextAsset jsonTextFile = Resources.Load<TextAsset>(path);

        wave = (Wave)JsonUtility.FromJson(jsonTextFile.text, typeof(Wave));
    }

    private void Update()
    {
        if (passedWaves >= wave.percentages.Count)
        {
            return;
        }

        if (lastProgress <= wave.percentages[passedWaves] &&
            GameManager.instance.levelProgress > wave.percentages[passedWaves])
        {
            Debug.Log($"Spawn wave {wave.sequences[passedWaves]} at {GameManager.instance.levelProgress}");

            SpawnWave();

            passedWaves++;
        }

        lastProgress = GameManager.instance.levelProgress;
    }

    void CreateEnemy(Vector3 offset, GameObject group)
    {
        Debug.Log("Spawn enemy");
        GameObject g = Instantiate(group);
        g.transform.position = transform.position + offset;
        Debug.Log("Spawned enemy");
    }

    void SpawnWave()
    {
        for (int seq = 0; seq < wave.sequences[passedWaves].Length; seq++)
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
    }

    IEnumerator SpawnSeq(float delay, List<Vector2> offset, List<string> paths)
    {
        Debug.Log("Cor started");
        yield return new WaitForSeconds(delay);
        Debug.Log("Cor waited");

        for (int i = 0; i < paths.Count; i++)
        {
            GameObject go = Resources.Load<GameObject>(paths[i]);

            CreateEnemy(offset[i], go);
        }
        Debug.Log("Cor ended");
    }
}
