using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public List<GameObject> keyList = new List<GameObject>();
    int nbPoint = 0;
    public class Marker
    {
        public Vector3 pos;
        public Quaternion rot;
        public Marker(Vector3 _pos, Quaternion _rot)
        {
            pos = _pos;
            rot = _rot;
        }
    }

    public List<Marker> markerList = new List<Marker>();

    //void Start()
    //{
    //    nbPoint = keyList[0].GetComponent<LineRenderer>().positionCount;
    //    Debug.Log(keyList[0].GetComponent<LineRenderer>().positionCount);
    //    for (int i = 0; i < nbPoint; i++)
    //    {
    //        markerList[i].pos = keyList[i].GetComponent<LineRenderer>().GetPosition(i);
    //        if (i > 0)
    //        {
    //            float angle = Vector3.AngleBetween(keyList[i].GetComponent<LineRenderer>().GetPosition(i), keyList[i + 1].GetComponent<LineRenderer>().GetPosition(i + 1));
    //            //markerList[i].rot = Vector3.
    //        }
    //
    //    }
    //}

    void FixedUpdate()
    {
        UpdateMarkerList();
    }

    public void UpdateMarkerList()
    {
        markerList.Add(new Marker(transform.position, transform.rotation));
    }
    public void clearMarkerList()
    {
        markerList.Clear();
        markerList.Add(new Marker(transform.position, transform.rotation));
    }


}
