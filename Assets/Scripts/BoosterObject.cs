using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterObject : MonoBehaviour
{
    public Booster booster;
    public SpriteRenderer back;
    public SpriteRenderer front;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Booster b)
    {
        booster = BoosterManager.instance.GetBoosterByRef(b);

        front.sprite = booster.sprite;
        front.color = booster.frontColor;

        back.sprite = BoosterManager.instance.GetRandomBack();

        if (booster.useRandomBackColor)
        {
            back.color = BoosterManager.instance.GetRandomColor();
        }
        else
        {
            back.color = booster.backColor;
        }

        gameObject.GetComponent<ScrollingSpeed>().SetScrollingBonusSpeed(booster.speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        booster.PickUpEvent.Invoke();
    }
}
