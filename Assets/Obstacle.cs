using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float randSpeed;
    // Start is called before the first frame update
    void Start()
    {
        float baseSpeed = gameObject.GetComponent<ScrollingSpeed>().currentSpeed;
        randSpeed = Random.Range(0, baseSpeed / 1.0f);
        float bonusSpeed = randSpeed;
        gameObject.GetComponent<ScrollingSpeed>().SetScrollingBonusSpeed(bonusSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
