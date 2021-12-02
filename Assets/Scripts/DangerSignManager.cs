 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSignManager : MonoBehaviour
{
    [SerializeField] Canvas dangerSignCanvas;
    [SerializeField] GameObject dangerSign;
    [SerializeField] float maxTimer;
    public static DangerSignManager instance;
    Vector2 canvasSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RectTransform canvasRect = dangerSignCanvas.GetComponent<RectTransform>();
        canvasSize = canvasRect.rect.size;
    }

    bool CheckIsInScreen(Vector3 _pos)
    {
        Vector3 pos = _pos;
        Vector3 size = transform.localScale;
        Vector3 bottomLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRightPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        if (pos.x + size.x / 2.0f < bottomLeftPos.x || pos.x - size.x / 2.0f > topRightPos.x ||
           pos.y + size.y / 2.0f < bottomLeftPos.y || pos.y - size.y / 2.0f > topRightPos.y
           )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //canvas version
    //give the enemyPos to calculate the dangerSign pos based on it
    //public void SpawnDangerSign(Vector3 _enemyPos, List<Vector2> _splinePointsPos)
    //public void SpawnDangerSign(Vector3 _enemyPos)
    //{
    //    Debug.Log("SpawnDangerSign");
    //    DangerSign currentSign = Instantiate(dangerSign).GetComponent<DangerSign>();
    //    RectTransform currentSignRect = currentSign.GetComponent<RectTransform>();
    //    Vector3 dangerSignPos = currentSignRect.position;
    //    currentSign.transform.SetParent(dangerSignCanvas.transform);
    //
    //    
    //    //which side the enemy is coming from
    //    if (_enemyPos.x < 0.0f)
    //    {
    //        dangerSignPos.x = currentSignRect.rect.width;
    //    }
    //    else
    //    {
    //        dangerSignPos.x = canvasSize.x - currentSignRect.rect.width;
    //    }
    //
    //    //get pos on canvas from enemy pos in world
    //    //dangerSignPos.y = canvasSize.y / 2 - Camera.main.WorldToScreenPoint(_enemyPos).y;
    //    dangerSignPos.y = canvasSize.y / 2;
    //
    //
    //    //for (int i = 0; i < _splinePointsPos.Count; i++)
    //    //{
    //    //    if (CheckIsInScreen(_splinePointsPos[i]))
    //    //    {
    //    //        Debug.Log("point = " + i);
    //    //        dangerSignPos = _splinePointsPos[i];
    //    //        break;
    //    //    }
    //    //}
    //
    //    //currentSignRect.position = Camera.main.WorldToScreenPoint(dangerSignPos);
    //    //Debug.Log(dangerSignPos);
    //    //Debug.Log(currentSignRect.position);
    //
    //
    //    //dangerSignPos.y = Camera.main.WorldToScreenPoint(dangerSignPos).y;
    //    currentSignRect.position = dangerSignPos;
    //    currentSign.SetTimer(maxTimer);
    //    
    //}

    public void SpawnDangerSign(Vector3 _enemyPos)
    {
        Debug.Log("SpawnDangerSign");
        DangerSign currentSign = Instantiate(dangerSign).GetComponent<DangerSign>();
        RectTransform currentSignRect = currentSign.GetComponent<RectTransform>();
        Vector3 dangerSignPos = currentSignRect.position;
        currentSign.transform.SetParent(dangerSignCanvas.transform);


        //which side the enemy is coming from
        if (_enemyPos.x < 0.0f)
        {
            dangerSignPos.x = currentSignRect.rect.width;
        }
        else
        {
            dangerSignPos.x = canvasSize.x - currentSignRect.rect.width;
        }

        //get pos on canvas from enemy pos in world
        dangerSignPos.y = canvasSize.y / 2;

        currentSign.canvasPos = dangerSignPos;
        currentSign.isEelDangerSign = false;
    }

    public DangerSign GetEelDangerSign(Vector3 _startPos)
    {
        DangerSign currentSign = Instantiate(dangerSign).GetComponent<DangerSign>();
        currentSign.gameObject.SetActive(false);
        currentSign.isEelDangerSign = true;

        return currentSign;
    }

    public ParticleSystem GetEelDangerSignParticleSystem(DangerSign _eelDangerSign)
    {
        ParticleSystem ps = _eelDangerSign.GetComponent<ParticleSystem>();
        //var main = ps.main;
        //main.loop = true;

        //disable for start
        ps.Stop();

        return ps;
    }

    public Vector2 GetEelDangerSignSize(DangerSign _eelDangerSign)
    {
        RectTransform rt = _eelDangerSign.GetComponent<RectTransform>();
        Vector2 size = Vector2.zero;

        //world size
        size = rt.lossyScale;
        return size;
    }
}
