using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<Coin>() != null 
            ||go.GetComponent<BoosterObject>() != null)
        {
            StartCoroutine(Player.MoveTowardPlayer(go.transform.position, go, 0.05f));
        }
    }
}
