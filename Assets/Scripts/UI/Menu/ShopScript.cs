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
        if (GameManager.instance.lives >= 5)
        {
            AudioManager.Instance.PlaySound("UIButtonError");
        }
        else
        {
            AudioManager.Instance.PlaySound("UIButton");
        }
        GameManager.instance.ChangeLives(1);

    }
    public void addLifeThree()
    {
        if (GameManager.instance.lives >= 5)
        {
            AudioManager.Instance.PlaySound("UIButtonError");
        }
        else
        {
            AudioManager.Instance.PlaySound("UIButton");
        }
        GameManager.instance.ChangeLives(3);

    }
    public void addLifeFive()
    {
        if (GameManager.instance.lives >= 5)
        {
            AudioManager.Instance.PlaySound("UIButtonError");
        }
        else
        {
            AudioManager.Instance.PlaySound("UIButton");
        }
        GameManager.instance.ChangeLives(5);

    }
    public void addMoneyOne00()
    {
        GameManager.instance.ChangeMoney(500);
        AudioManager.Instance.PlaySound("UIButton");

    }
    public void addMoneyTwo00()
    {
        GameManager.instance.ChangeMoney(1000);
        AudioManager.Instance.PlaySound("UIButton");

    }
    public void addMoneyFour00()
    {
        GameManager.instance.ChangeMoney(2000);
        AudioManager.Instance.PlaySound("UIButton");

    }
}
