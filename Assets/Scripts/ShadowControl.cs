using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowControl : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject parent;
    [SerializeField] bool isYInverted = true;
    // Update is called once per frame

    private void Start()
    {
        if (isYInverted)
        {
            offset.y *= -1;
        } 
    }

    void Update()
    {
        transform.position = parent.transform.position + offset;
        transform.rotation = parent.transform.rotation;
    }
}
