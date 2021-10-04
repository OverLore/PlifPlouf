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
        float offset = 0.1f;
        if (pos.y + size.y / 2.0f + offset < topRightPos.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void AddBackgroundInList(Vector3 _pos)
    {
        GameObject go = Instantiate(background);
        go.transform.position = _pos;
        backgroundList.Add(go.GetComponent<Background>());
    }

    // Start is called before the first frame update
    void Start()
    {
        AddBackgroundInList(new Vector3 (0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        for (int i = 0; i < backgroundList.Count; i++)
        {
            if (IsLowerThanTop(backgroundList[i]) && !backgroundList[i].hasCreatedNext)
            {
                //AddBackgroundInList(new Vector3(0, 10, 0));
                counter++;
            }
        }

        for (int i = 0; i < counter; i++)
        {
            if (IsLowerThanTop(backgroundList[i]) && !backgroundList[i].hasCreatedNext)
            {
                AddBackgroundInList(new Vector3(0, 10, 0));
                backgroundList[i].hasCreatedNext = true;
            }
        }

    }
}
