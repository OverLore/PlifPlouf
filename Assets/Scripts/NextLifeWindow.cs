using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextLifeWindow : Window
{
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] TextMeshProUGUI nextIn;

    void Update()
    {
        amount.text = GameManager.instance.lives.ToString();

        System.TimeSpan diff = GameManager.instance.nextLifeAt.Subtract(System.DateTime.Now);
        nextIn.text = diff.ToString("mm':'ss");
    }
}
