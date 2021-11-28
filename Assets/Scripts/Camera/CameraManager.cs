using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    //size of white rect camera on the xiaomi redmi note 7 in pixels (useful for conversion)
    private Vector2 refRectSizePixels = new Vector2(462, 1000); //unity units : 4.615385f, 10
    //multiply your size by this to go from units to pixels (only work with orthographic camera and camera size 5)
    private Vector2 pixelsUnitsRatio = new Vector2(100.1f, 100);
    //size of the whole game in pixels (2 whole screen wide, 1 screen height)
    private Vector2 refGameSizePixels = new Vector2(462 * 2, 1000);
    //current camera rect size in unity pixels
    public Vector2 cameraRectSizePixels;
    //current camera rect size in unity units
    public Vector2 cameraRectSizeUnits;
    //ratio of the camera (current / ref) (in pixels) (multiply the size to this to go from ref camera size to current camera size)
    public Vector2 camerasAspectRatio;
    //same size on x and y (for uniform scaling)
    public Vector2 camerasAspectRatioUniform;
    public Vector2 refScreenSize = new Vector2(1080.0f, 2340.0f);

    //use this function if you want to make your gameObject (containing a sprite) scaling to the camera size
    //ex : make a background sprite covering the whole camera vision (screen) whatever the resolution
    public void SetSpriteScaleToCameraSize(GameObject _go)
    {
        SpriteRenderer sr = _go.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = _go.GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
            {
                Debug.Log("no sprite renderer found");
                return;
            }
        }
        Vector3 size = sr.bounds.size;
        //deformation since not the same ratio on x and y of scaling
        //Vector3 newSize = size * camerasAspectRatio;
        //uniform scaling
        Vector3 newSize = size * camerasAspectRatioUniform;
        Vector3 scaleRatio = new Vector3(newSize.x / size.x, newSize.y / size.y, 0);
        _go.transform.localScale = new Vector3(_go.transform.localScale.x * scaleRatio.x, _go.transform.localScale.y * scaleRatio.y, 0);
    }

    //public void SetImageScaleToCameraSize(GameObject _go)
    //{
    //    Image image = _go.GetComponent<Image>();
    //    if (image == null)
    //    {
    //        if (image == null)
    //        {
    //            Debug.Log("no sprite renderer found");
    //            return;
    //        }
    //    }
    //    Vector3 currentSize = image.rectTransform.localScale;
    //    //deformation since not the same ratio on x and y of scaling
    //    //Vector3 newSize = size * camerasAspectRatio;
    //    //uniform scaling
    //    Vector3 newSize = currentSize * camerasAspectRatio;
    //    Vector3 scaleRatio = new Vector3(newSize.x / currentSize.x, newSize.y / currentSize.y, 0);
    //    image.rectTransform.localScale = new Vector3(_go.transform.localScale.x * scaleRatio.x, _go.transform.localScale.y * scaleRatio.y, 0);
    //}

    public void SetCanvasScaleToCameraSize(GameObject _go)
    {
        CanvasScaler cs = _go.GetComponent<CanvasScaler>();
        if (cs == null)
        {
            Debug.Log("no sprite renderer found");
            return;
        }
        float currentScale = cs.scaleFactor;
        //deformation since not the same ratio on x and y of scaling
        //Vector3 newSize = size * camerasAspectRatio;
        //uniform scaling
        Vector2 resRatio =new Vector2(Screen.width, Screen.height) / refScreenSize;
        float uniformScale = resRatio.x > resRatio.y ? resRatio.x : resRatio.y;
        cs.scaleFactor = uniformScale;
    }

    //public void SetCanvasScaleToCameraSize(GameObject _go)
    //{
    //    CanvasScaler cs = _go.GetComponent<CanvasScaler>();
    //    if (cs == null)
    //    {
    //        Debug.Log("no sprite renderer found");
    //        return;
    //    }
    //    Vector2 currentRes = cs.referenceResolution;
    //    //deformation since not the same ratio on x and y of scaling
    //    //Vector3 newSize = size * camerasAspectRatio;
    //    //uniform scaling
    //    Vector2 newRes = currentRes / camerasAspectRatioUniform;
    //    cs.referenceResolution = newRes;
    //}

    // Start is called before the first frame update
    void Awake()
    {
        //get current camera size in unity units
        cameraRectSizeUnits.y = 2 * Camera.main.orthographicSize;
        cameraRectSizeUnits.x = cameraRectSizeUnits.y * Camera.main.aspect;
        //get current camera size in pixels
        cameraRectSizePixels.y = cameraRectSizeUnits.y * pixelsUnitsRatio.y;
        cameraRectSizePixels.x = cameraRectSizeUnits.x * pixelsUnitsRatio.x;
        //get the camera scaling ratio
        camerasAspectRatio = cameraRectSizePixels / refRectSizePixels;
        //get uniform ratio (larger of the two components)
        if (camerasAspectRatio.x >= camerasAspectRatio.y)
        {
            camerasAspectRatioUniform.x = camerasAspectRatio.x;
            camerasAspectRatioUniform.y = camerasAspectRatio.x;
        }
        else
        {
            camerasAspectRatioUniform.x = camerasAspectRatio.y;
            camerasAspectRatioUniform.y = camerasAspectRatio.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
