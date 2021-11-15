using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    CameraManager cameraManager;
    public  Vector2 targetPos;
    Vector2 startPos;
    Vector2 currentPos;
    //one second to go from current pos  to next Pos
    float moveTime = 1.0f;
    float currentMoveTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = gameObject.GetComponent<CameraManager>();
        startPos = gameObject.transform.position;
        targetPos = startPos;
        currentPos = startPos;
        currentMoveTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(startPos != targetPos)
        {
            currentPos.x = Mathf.Lerp(startPos.x, targetPos.x, currentMoveTime);
            currentPos.y = Mathf.Lerp(startPos.y, targetPos.y, currentMoveTime);

            Camera.main.transform.position = new Vector3(currentPos.x, currentPos.y, 0.0f);
            ClampCamera();

            currentMoveTime += Time.deltaTime * GameManager.instance.timeScale;
            if(currentMoveTime > 1.0f)
            {
                currentMoveTime = 1.0f;
            }
        }
    }

    void ClampCamera()
    {
        float xMin = -cameraManager.cameraRectSizeUnits.x / 2;
        float xMax = cameraManager.cameraRectSizeUnits.x / 2;

        float yMin = 0;
        float yMax = 0;

        //Debug.Log("cam" + Camera.main.transform.position);
        //Debug.Log("xMin" + xMin);
        //Debug.Log("xMax" + xMax);
        Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, xMin, xMax),
            Mathf.Clamp(Camera.main.transform.position.y, yMin, yMax), Camera.main.transform.position.z);
    }
}
