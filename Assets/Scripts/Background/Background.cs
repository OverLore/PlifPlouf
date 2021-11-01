using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public bool hasCreatedNext = false;
    public int index;
    public ScrollingSpeed scrollingComponent;

    void Start()
    {
        scrollingComponent = gameObject.GetComponent<ScrollingSpeed>();
    }

    private void OnDestroy()
    {
        //disable the children scripts for no reasons (same for parent = null)
        gameObject.transform.DetachChildren();
        //fix by hand (no other solution to enable the scripts)
        BackgroundManager.instance.backgroundList[index + 1].GetComponent<Background>().enabled = true;
        BackgroundManager.instance.backgroundList[index + 1].GetComponent<ScrollingSpeed>().enabled = true;

        BackgroundManager.instance.backgroundList[index + 1].scrollingComponent.isScrolling = true;
    }
}
