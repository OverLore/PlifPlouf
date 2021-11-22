using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    Rigidbody2D rb;
    Vector2 vel;

    void DestroyBullet(bool _playDestroySound)
    {
        if (_playDestroySound)
        {
            AudioManager.Instance.PlaySound("MeduseBubblePop");
        }
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Ennemy")
        {
            //Destroy(collision.gameObject);
            
            collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
            DestroyBullet(true);
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
        rb = GetComponent<Rigidbody2D>();

        vel = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = vel * GameManager.instance.timeScale;

        if (!CheckIsInScreen())
        {
            DestroyBullet(false);
        }
    }
}