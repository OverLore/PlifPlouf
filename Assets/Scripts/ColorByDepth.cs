using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorByDepth : MonoBehaviour
{
    [SerializeField] int layer;
    [SerializeField] SpriteRenderer spr;

    private void Start()
    {
        if (spr == null)
        {
            spr = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        Color tempColor = spr.color;
        tempColor = GameManager.instance.colorByLayers[layer];
        tempColor.a = spr.color.a;
        spr.color = tempColor;
    }
}
