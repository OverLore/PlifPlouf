using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNoel : MonoBehaviour
{
    [SerializeField] Sprite normalMap;
    [SerializeField] Sprite noelMap;
    [SerializeField] Image img;

    private void Start()
    {
        if (Database.SeasonSkin != null && Database.SeasonSkin.is_activated)
        {
            img.sprite = noelMap;
        }
        else
        {
            img.sprite = normalMap;
        }
    }
}
