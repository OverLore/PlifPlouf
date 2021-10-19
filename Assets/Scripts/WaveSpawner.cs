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

    void CreateEnemy(string animation, Vector3 offset, GameObject fish)
    {
        GameObject par = Instantiate(fishPrefab);
        GameObject pattern = par.transform.GetChild(0).gameObject;

        par.transform.position = transform.position + offset;

        Animator patternAnimator = pattern.GetComponent<Animator>();
        patternAnimator.SetTrigger(animation);

        GameObject fishObj = Instantiate(fish);
        fishObj.transform.parent = pattern.transform;
        fishObj.transform.localPosition = Vector3.zero;
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

                StartCoroutine(SpawnSeq(delay, sequence[i].Pattern, sequence[i].Offset, sequence[i].Fish));
            }
        }
    }

    IEnumerator SpawnSeq(float delay, List<string> animation, List<Vector2> offset, List<GameObject> fish)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < animation.Count; i++)
        {
            CreateEnemy(animation[i], offset[i], fish[i]);
        }
    }
}
