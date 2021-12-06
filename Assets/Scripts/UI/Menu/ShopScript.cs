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
    public void addMoneyOne00()
    {
        GameManager.instance.ChangeMoney(100);

    }
    public void addMoneyTwo00()
    {
        GameManager.instance.ChangeMoney(200);

    }
    public void addMoneyFour00()
    {
        GameManager.instance.ChangeMoney(400);

    }
}
