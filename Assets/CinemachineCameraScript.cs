using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraScript : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer framingTransposer;
    [SerializeField] float screenX = 0.5f;
    [SerializeField] float screenY = 0.5f;
    [SerializeField] float deadZoneWidth = 0.6f;
    [SerializeField] float deadZoneHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        //lock movement of screen X and Y (don't know a better method)
        framingTransposer.m_ScreenX = screenX;
        framingTransposer.m_ScreenY = screenY;
        framingTransposer.m_DeadZoneWidth = deadZoneWidth;
        framingTransposer.m_DeadZoneHeight = deadZoneHeight;
    }
}
