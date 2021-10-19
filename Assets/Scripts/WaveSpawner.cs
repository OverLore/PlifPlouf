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
        string path = $"Assets/Resources/Levels/{levelToLoad}.json";

        FileInfo fi = new FileInfo(path);
        if (!fi.Directory.Exists)
        {
            Debug.LogError($"Niveau {levelToLoad} n'existe pas");

            return;
        }

        path = $"Levels/{levelToLoad}";

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
        GameObject g = Instantiate(group);
        g.transform.position = transform.position + offset;
    }

    void SpawnWave()
    {
        for (int seq = 0; seq < wave.sequences[passedWaves].Length; seq++)
        {
            List<WaveSequence> sequence = new List<WaveSequence>();

            string path = $"Assets/Resources/Waves/{wave.sequences[passedWaves]}/";

            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                return;
            }

            path = $"Waves/{wave.sequences[passedWaves]}/";

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

                StartCoroutine(SpawnSeq(delay, sequence[i].Offset, sequence[i].Group));
            }
        }
    }

    IEnumerator SpawnSeq(float delay, List<Vector2> offset, List<GameObject> group)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < group.Count; i++)
        {
            CreateEnemy(offset[i], group[i]);
        }
    }
}
