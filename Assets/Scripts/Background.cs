using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public bool hasCreatedNext = false;

    // Start is called before the first frame update
    void Awake()
    {
        //float sizeDivide = 3.0f;
        //SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        //if (sr == null)
        //{
        //    sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        //    if (sr == null)
        //    {
        //        Debug.Log("no sprite renderer found");
        //    }
        //}
        //sr.bounds.size.Set(sr.bounds.size.x / sizeDivide, sr.bounds.size.y / sizeDivide, 0);
        //transform.localScale = new Vector3(transform.localScale.x / sizeDivide, transform.localScale.y / sizeDivide, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
