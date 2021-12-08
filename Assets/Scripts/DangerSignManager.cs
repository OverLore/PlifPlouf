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
    Vector2 screenSize;

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
        screenSize = new Vector2(-1, -1);
    }

    private void Update()
    {
        if (screenSize.x == -1)
        {
            screenSize = new Vector2(ScreenSize.GetScreenToWorldWidth / 2, ScreenSize.GetScreenToWorldHeight / 2);
        }
    }

    //do not use for eel danger signs
    public void SpawnDangerSign(Transform _enemyTransform)
    {
        DangerSign currentSign = Instantiate(dangerSign).GetComponent<DangerSign>();
        RectTransform currentSignRect = currentSign.GetComponent<RectTransform>();
        Vector3 dangerSignPos = currentSignRect.position;
        currentSign.transform.SetParent(dangerSignCanvas.transform);
        Vector2 warningSignSize = GetEelDangerSignSize(currentSign);

        currentSign.SetTarget(_enemyTransform);
        currentSign.warningSignSize = warningSignSize;
        currentSign.transform.position = GetDangerSignPosByHeadPos(_enemyTransform, warningSignSize);

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

    //duplicata of method in EelMove.cs to reuse it in murene.cs (without destroying game by misinterpreting the code)
    public Vector2 GetDangerSignPosByHeadPos(Transform head, Vector2 warningSignSize)
    {
        Vector3 pos = Vector3.zero;

        //Debug.Log(screenSize);
        //the head is :
        //over right of the screen
        if (head.position.x >= -screenSize.x + Camera.main.transform.position.x)
        {
            pos.x = screenSize.x + Camera.main.transform.position.x - warningSignSize.x / 2;
            //under the screen
            if (head.position.y <= -screenSize.y + Camera.main.transform.position.y)
            {
                pos.y = -screenSize.y + Camera.main.transform.position.y + warningSignSize.y;
            }
            //over the screen
            else if (head.position.y >= screenSize.y + Camera.main.transform.position.y)
            {
                pos.y = screenSize.y + Camera.main.transform.position.y - warningSignSize.y;
            }
            //in between
            else
            {
                pos.y = head.position.y;
            }
        }
        //over the left of the screen
        else
        {
            pos.x = -screenSize.x + Camera.main.transform.position.x + warningSignSize.x / 2;

            //under the screen
            if (head.position.y <= -screenSize.y + Camera.main.transform.position.y)
            {
                pos.y = -screenSize.y + Camera.main.transform.position.y + warningSignSize.y;
            }
            //over the screen
            else if (head.position.y >= screenSize.y + Camera.main.transform.position.y)
            {
                pos.y = screenSize.y + Camera.main.transform.position.y - warningSignSize.y;
            }
            //in between
            else
            {
                pos.y = head.position.y;
            }
        }

        return pos;
    }
}
