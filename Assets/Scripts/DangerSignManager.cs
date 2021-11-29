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

    //give the enemyPos to calculate the dangerSign pos based on it
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
        //dangerSignPos.y = Camera.main.WorldToScreenPoint(_enemyPos).y;
        dangerSignPos.y = canvasSize.y / 2;
        currentSignRect.position = dangerSignPos;

        currentSign.SetTimer(maxTimer);
        
    }
}
