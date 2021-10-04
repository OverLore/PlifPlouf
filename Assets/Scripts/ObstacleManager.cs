using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    [SerializeField] float timer;
    [SerializeField] float maxTimer;
    [SerializeField] int nbObstacles;
    [SerializeField] int nbRandObstacles;
    [SerializeField] int nbObstaclesToSpawn;


    // Start is called before the first frame update
    void Start()
    {
        timer = maxTimer;
    }

    int GetObstaclesNumberAtSpawn()
    {
        //max is exclusive
        int result = Random.Range(nbObstacles, nbObstacles + nbRandObstacles + 1);
        return result;
    }

    void SpawnObstacles()
    {
        int randNbObstacles = GetObstaclesNumberAtSpawn();
        for (int i = 0; i < randNbObstacles; i++)
        {
            if (nbObstaclesToSpawn > 0)
            {
                float randXPos = Random.Range(-1.8f, 1.8f);
                GameObject go = Instantiate(obstacle);
                go.transform.position = new Vector3(randXPos, 8, 0);
                nbObstaclesToSpawn -= 1;
            }
            else
            {
                break;
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            SpawnObstacles();
            timer = maxTimer;
        }
    }
}
