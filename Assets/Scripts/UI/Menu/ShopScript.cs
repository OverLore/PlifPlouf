using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addLifeOne()
    {
        GameManager.instance.ChangeLives(1);

    }
    public void addLifeThree()
    {
        GameManager.instance.ChangeLives(3);

    }
    public void addLifeFive()
    {
        GameManager.instance.ChangeLives(5);

    }
}
