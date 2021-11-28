using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScreenScaler : MonoBehaviour
{
    [SerializeField] GameObject[] canvasArray;

    // Start is called before the first frame update
    void Start()
    {
        CameraManager cm = Camera.main.GetComponent<CameraManager>();
        foreach (GameObject canvas in canvasArray)
        {
            cm.SetCanvasScaleToCameraSize(canvas);
        }
    }
}
