using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardLine : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text idText;
    [SerializeField] TMP_Text valueText;

    public void Setup(int id, string profile, int value)
    {
        nameText.text = profile;
        idText.text = id.ToString();
        valueText.text = value.ToString();
    }
}
