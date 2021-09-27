using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    public float shotForce;

    float nextShot;
    float delay;

    // Start is called before the first frame update
    void Start()
    {
        nextShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        nextShot -= Time.deltaTime;

        if (nextShot < 0)
        {
            nextShot = delay;

            GameObject go = Instantiate(bullet);

            go.transform.position = transform.position;

            go.GetComponent<Rigidbody2D>().velocity = Vector3.up * shotForce;
        }
    }
}
