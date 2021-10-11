using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRay : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            //Destroy(collision.gameObject);

            collision.gameObject.GetComponent<Enemy>().takeDamage(1);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
