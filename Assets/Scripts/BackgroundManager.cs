using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] GameObject background;
    public List<Background> backgroundList = new List<Background>();

    bool IsLowerThanTop(Background _go)
    {
        Vector3 pos = _go.transform.position;
        Vector3 size = _go.transform.localScale;
        Vector3 topRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float offset = -1.0f;
        if (pos.y + size.y / 2.0f + offset > topRightPos.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void AddBackgroundInList(int _addedIndex)
    {
        GameObject go = Instantiate(background);
        Vector3 pos = new Vector3(0, 0, 0);
        int lastIndex = _addedIndex - 1;
        if(_addedIndex != 0)
        {
            backgroundList[lastIndex].hasCreatedNext = true;
            Vector3 lastPos = backgroundList[lastIndex].transform.position;
            Vector3 lastSize = backgroundList[lastIndex].transform.localScale;
            pos = new Vector3(lastPos.x, lastPos.y + lastSize.y, lastPos.z);
        }
        go.transform.position = pos;
        backgroundList.Add(go.GetComponent<Background>());
    }

    // Start is called before the first frame update
    void Start()
    {
        AddBackgroundInList(0);
        AddBackgroundInList(1);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgroundList.Count; i++)
        {
            if(!backgroundList[i].hasCreatedNext && IsLowerThanTop(backgroundList[i]))
            {
                AddBackgroundInList(i + 1);
                break;
            }
        }

        
        //int counter = 0;
        //for (int i = 0; i < backgroundList.Count; i++)
        //{
        //    if (IsLowerThanTop(backgroundList[i]) && !backgroundList[i].hasCreatedNext)
        //    {
        //        counter++;
        //        backgroundList[i].hasCreatedNext = true;
        //        backgroundList.Remove(backgroundList[i]);
        //        i--;
        //    }
        //}
        //
        //for (int i = 0; i < counter; i++)
        //{
        //    AddBackgroundInList(new Vector3(0, 10, 0));
        //}
        //
    }
}
