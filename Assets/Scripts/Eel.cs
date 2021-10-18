using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel : MonoBehaviour
{
    [SerializeField] float distanceBetween;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] List<GameObject> bodyPart = new List<GameObject>();
    List<GameObject> eelBody = new List<GameObject>();

    //bool count = false;
    float countUp = 0;
    // Start is called before the first frame update
    void Start()
    {
        countUp = distanceBetween;
        CreatBodyPart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bodyPart.Count > 1)
        {
            CreatBodyPart();
        }
        EelMovement();
    }

    void EelMovement()
    {
        eelBody[0].GetComponent<Rigidbody2D>().velocity = eelBody[0].transform.right * speed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") != 0)
        {
            eelBody[0].transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));
        }
        if (eelBody.Count > 1)
        {
            for (int i = 0; i < eelBody.Count; i++)
            {
                MarkerManager key = eelBody[i - 1].GetComponent<MarkerManager>();
                eelBody[i].transform.position = key.markerList[0].pos;
                eelBody[i].transform.rotation = key.markerList[0].rot;
                key.markerList.RemoveAt(0);
                //Debug.Log(i);
            }
        }     
    }

    void CreatBodyPart()
    {
        if (eelBody.Count == 0)
        {
            GameObject temp = Instantiate(bodyPart[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            eelBody.Add(temp);
            bodyPart.RemoveAt(0);
        }
        
        MarkerManager key = eelBody[eelBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            key.clearMarkerList();
        }
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyPart[0], key.markerList[0].pos, key.markerList[0].rot);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            eelBody.Add(temp);
            bodyPart.RemoveAt(0);
            temp.GetComponent<MarkerManager>().clearMarkerList();
            countUp = 0;
        }
    }
}