using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance = null;
    [SerializeField] GameObject background;
    [SerializeField] GameObject backgroundNoel;
    GameObject Background { get { return xmas ? backgroundNoel : background; } }
    public List<Background> backgroundList = new List<Background>();
    private PolygonCollider2D gameBorders;
    private CameraManager cameraManager;
    [SerializeField] CinemachineConfiner cinemachineConfiner;

    [Space(10), Header("Decors")]
    [SerializeField] float minXOffset;
    [SerializeField] float maxXOffset;
    GameObject[] decors;
    [SerializeField] GameObject normalStart;
    [SerializeField] GameObject xmasStart;
    float nextDecorIn = 0f;

    bool xmas;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            if (Application.isPlaying)
                Destroy(gameObject);
        }

        xmas = Database.SeasonSkin != null && Database.SeasonSkin.is_activated;

        if (xmas)
        {
            Instantiate(xmasStart);
            decors = Resources.LoadAll<GameObject>("Prefabs/DecorNoel");
        }
        else
        {
            Instantiate(normalStart);
            decors = Resources.LoadAll<GameObject>("Prefabs/Decor");
        }
    }

    bool IsLowerThanTop(Background _go)
    {
        Vector3 pos = _go.transform.position;
        float sizeY = _go.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 topRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float offset = -1.0f;
        if (pos.y + sizeY / 2.0f + offset < topRightPos.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AddBackgroundInList(int _addedIndex)
    {
        GameObject go = Instantiate(Background);
        float sizeDivide = 3.0f;

        //background is 3 times bigger than "ref" resolution (so that every resolution of phone/tablet
        //won't have to upscale the background) (why is it working for setSpriteScaleToCameraSize but not for this ??) (bounds keep etc..)
        //go.transform.localScale = new Vector3(go.transform.localScale.x / sizeDivide, go.transform.localScale.y / sizeDivide, 0.0f);
        //change scale to fit every resolution of screens
        go.transform.localScale /= sizeDivide;
        Camera.main.GetComponent<CameraManager>().SetSpriteScaleToCameraSize(go);

        Vector3 pos = new Vector3(0, 0, 0);
        int lastIndex = _addedIndex - 1;
        if (_addedIndex != 0)
        {
            backgroundList[lastIndex].hasCreatedNext = true;
            Vector3 lastPos = backgroundList[lastIndex].transform.position;
            float sizeY = backgroundList[lastIndex].GetComponent<SpriteRenderer>().bounds.size.y;
            pos = new Vector3(lastPos.x, lastPos.y + sizeY, lastPos.z);
            go.transform.transform.SetParent(backgroundList[lastIndex].transform);
            go.GetComponent<ScrollingSpeed>().isScrolling = false;
        }
        go.transform.position = pos;
        go.GetComponent<Background>().index = _addedIndex;
        
        backgroundList.Add(go.GetComponent<Background>());
    }

    void SetGameBordersSizeToCameraSize()
    {
        //cinemachineConfiner = Camera.main.GetComponent<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
        gameBorders = gameObject.GetComponent<PolygonCollider2D>();
        cameraManager = Camera.main.GetComponent<CameraManager>();
        Vector2 ratio = cameraManager.camerasAspectRatio;
        Vector2[] points = gameBorders.GetPath(0);
        for (int i = 0; i < points.Length; i++)
        {
            points[i].Set(points[i].x * ratio.x, points[i].y * ratio.y);
        }
        gameBorders.SetPath(0, points);
        cinemachineConfiner.m_BoundingShape2D = gameBorders;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameBordersSizeToCameraSize();

        AddBackgroundInList(0);
        //AddBackgroundInList(1);
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

        nextDecorIn -= Time.deltaTime * GameManager.instance.timeScale;

        if (nextDecorIn < 0)
        {
            int nbToSpawn = Random.Range(1, 2);

            for (int i = 0; i < nbToSpawn; i++)
            {
                GameObject go = Instantiate(decors[Random.Range(0, decors.Length)]);
                go.transform.position = new Vector3(Random.Range(minXOffset, maxXOffset), 6, 0);
            }

            nextDecorIn = Random.Range(.4f, .8f);
        }
    }
}
