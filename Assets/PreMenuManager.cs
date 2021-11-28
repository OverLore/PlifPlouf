using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScreenScaling : MonoBehaviour
{
    [SerializeField] GameObject preMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        CameraManager cm = Camera.main.GetComponent<CameraManager>();
        cm.SetCanvasScaleToCameraSize(preMenuCanvas);
    }
}
