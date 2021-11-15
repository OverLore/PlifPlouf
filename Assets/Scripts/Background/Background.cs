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
        //(Evan) le background se fait delete in game si on fait ça, bien que chez toi ça marche
        //if (Application.isEditor)
        //    return;
        
        //disable the children scripts for no reasons (same for parent = null)
        gameObject.transform.DetachChildren();
        //fix by hand (no other solution to enable the scripts)

        if (index + 1 < 0 || index + 1 >= BackgroundManager.instance.backgroundList.Count)
        {
            return;
        }

        BackgroundManager.instance.backgroundList[index + 1].GetComponent<Background>().enabled = true;
        BackgroundManager.instance.backgroundList[index + 1].GetComponent<ScrollingSpeed>().enabled = true;

        BackgroundManager.instance.backgroundList[index + 1].scrollingComponent.isScrolling = true;
    }
}
