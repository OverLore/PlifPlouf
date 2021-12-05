using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextLifeWindow : Window
{
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] TextMeshProUGUI nextInLabel;
    [SerializeField] TextMeshProUGUI nextIn;

    void Update()
    {
        amount.text = GameManager.instance.lives.ToString();

        if (GameManager.instance.lives < 5)
        {
            nextInLabel.text = "1 free life in :";
            System.TimeSpan diff = GameManager.instance.nextLifeAt.Subtract(System.DateTime.Now);
            nextIn.text = diff.ToString("mm':'ss");
        }
        else
        {
            nextInLabel.text = "full";
            nextIn.text = "";
        }
    }
}
