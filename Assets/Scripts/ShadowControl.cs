using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowControl : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject parent;

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.transform.position + offset;
        transform.rotation = parent.transform.rotation;
    }
}
