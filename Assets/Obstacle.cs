using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float randSpeed;
    [SerializeField] float rotateSpeed = 1.0f;
    [SerializeField] float obstacleDamage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        float baseSpeed = gameObject.GetComponent<ScrollingSpeed>().currentSpeed;
        randSpeed = Random.Range(0, baseSpeed / 1.0f);
        float bonusSpeed = randSpeed;
        gameObject.GetComponent<ScrollingSpeed>().SetScrollingBonusSpeed(bonusSpeed);
    }

    void RotateObstacle()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        RotateObstacle();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision with player
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(obstacleDamage);
        }
    }
}
