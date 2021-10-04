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

    private int baseNbObstaclesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxTimer;

        //keep the values for the reload
        baseNbObstaclesToSpawn = nbObstaclesToSpawn;
        Debug.Log(baseNbObstaclesToSpawn);
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

    void SpawnObstacles()
    {
        int randNbObstacles = GetObstaclesNumberAtSpawn();
        for (int i = 0; i < randNbObstacles; i++)
        {
            if (nbObstaclesToSpawn > 0)
            {
                float randXPos = Random.Range(-1.8f, 1.8f);
                float randYCoeff = Random.Range(3.0f, 5.0f);
                float yPos = 8 + randYCoeff * i;
                GameObject go = Instantiate(obstacle);
                go.transform.position = new Vector3(randXPos, yPos, 0);
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

        if(Input.GetKey(KeyCode.Space))
        {
            RestartComponents();
            Debug.Log("restart components");
        }
    }
}
