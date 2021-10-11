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
    [SerializeField] int maxSpawnColumns = 5;

    private int baseNbObstaclesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxTimer;

        //keep the values for the reload
        baseNbObstaclesToSpawn = nbObstaclesToSpawn;
    }

    //reload components for a new wave
    void RestartComponents()
    {
        nbObstaclesToSpawn = baseNbObstaclesToSpawn;
    }

    int GetObstaclesNumberAtSpawn()
    {
        //max is exclusive
        int result = Random.Range(nbObstacles, nbObstacles + nbRandObstacles + 1);
        return result;
    }

    //small implementation, will be held by Luc's wave editor
    void SpawnObstacles()
    {
        int randNbObstacles = GetObstaclesNumberAtSpawn();
        for (int i = 0; i < randNbObstacles; i++)
        {
            if (nbObstaclesToSpawn > 0)
            {
                float randColumn = Random.Range(0, maxSpawnColumns) + 1;
                float randXPos = Random.Range(-3.6f, 3.6f);
                float randYCoeff = Random.Range(3.0f, 5.0f);
                float yPos = 8 + randYCoeff * i;
                GameObject go = Instantiate(obstacle);
                go.transform.position = new Vector3(randXPos, yPos, 0);
                nbObstaclesToSpawn -= 1;
            }
            else
            {
                //for the moment it will loop
                RestartComponents();
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
