using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //CameraMovement cameraMovement;
    Vector3 lastPos;
    Vector3 dP;

    bool start;

    private void Start()
    {
        //cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) && Input.touchCount != 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
        
                if (touch.phase == TouchPhase.Began)
                {
                    start = true;

                    dP = Vector2.zero;
                    lastPos = Vector2.zero; 

                    if (Input.touchCount > 1)
                    {
                        return;
                    }
                }
        
                //if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y));
        
                    dP = point - lastPos;
        
                    if (start)
                    {
                        dP = Vector2.zero;
        
                        start = false;
                    }
        
                    transform.position += new Vector3(dP.x, dP.y, 0); 
                    //cameraMovement.targetPos += new Vector2(dP.x, dP.y);


                    ClampPlayer();
        
                    lastPos = point;
                }
        
                if (touch.phase == TouchPhase.Ended)
                {
                    dP = Vector2.zero;
        
                    start = true;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                start = true;

                dP = Vector2.zero;
                lastPos = Vector2.zero;

                if (Input.touchCount > 1)
                {
                    return;
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

                dP = point - lastPos;

                if (start)
                {
                    dP = Vector2.zero;

                    start = false;
                }

                transform.position += new Vector3(dP.x, dP.y, 0);
                //cameraMovement.targetPos += new Vector2(dP.x, dP.y);

                ClampPlayer();

                lastPos = point;
            }

            if (Input.GetMouseButtonUp(0))
            {
                dP = Vector2.zero;

                start = true;
            }
        }
    }

    void ClampPlayer()
    {
        float xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + transform.localScale.x / 2;
        float xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - transform.localScale.x / 2;

        float yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + transform.localScale.y / 2;
        float yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - transform.localScale.y / 2;

        Debug.Log(xMax);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
    }
}
