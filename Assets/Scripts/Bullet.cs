using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            //Destroy(collision.gameObject);
            
            collision.gameObject.GetComponent<Enemy>().takeDamage(1);
            Destroy(gameObject);
        }
    }
    bool CheckIsInScreen()
    {
        Vector3 pos = transform.position;
        Vector3 size = transform.localScale;
        Vector3 bottomLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        if (pos.x + size.x / 2.0f < bottomLeftPos.x || pos.x - size.x / 2.0f > topRightPos.x ||
           pos.y + size.y / 2.0f < bottomLeftPos.y || pos.y - size.y / 2.0f > topRightPos.y
           )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckIsInScreen())
        {
            Destroy(gameObject);
        }
    }
}