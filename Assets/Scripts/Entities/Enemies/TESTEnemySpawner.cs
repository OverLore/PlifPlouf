using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    //public Transform pos;

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

           // go.transform.position = pos.transform.position;
        }
        else if (nextIn <= 0 && rest <= 0)
        {
       
            // pos.transform.position = pos.transform.position + new Vector3(transform.position.x - 50, transform.position.y , transform.position.z);
            //gameObject.transform.position = gameObject.transform.position + new Vector3(gameObject.transform.position.x - 50, gameObject.transform.position.y, gameObject.transform.position.z);
            rest = 15;
            nextIn =3f;
        }
    }
}
