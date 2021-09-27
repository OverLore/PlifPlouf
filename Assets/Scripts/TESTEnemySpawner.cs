using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Transform pos;

    float nextIn;
    int rest = 15;

    // Start is called before the first frame update
    void Start()
    {
        nextIn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        nextIn -= Time.deltaTime;

        if (nextIn <= 0 && rest >= 0)
        {
            rest--;

            nextIn = 0.1f;

            GameObject go = Instantiate(enemyPrefab);

            go.transform.position = pos.transform.position;
        }
    }
}
