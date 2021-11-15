using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSpeed : MonoBehaviour
{
    //depth layer will determine the scrolling speed in an idea of parallax implementation
    [SerializeField] int depthLayer;
    public float baseSpeed;
    public float currentSpeed;
    [SerializeField] private float bonusSpeed = 0.0f;
    public bool isScrolling = true;

    bool CheckIsHigherThanBottom()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if(sr == null)
        {
            sr = gameObject.GetComponentInChildren<SpriteRenderer>();
            if(sr == null)
            {
                Debug.Log("no sprite renderer found");
                return false;
            }
        }
        Vector3 pos = transform.position;
        float sizeY = sr.bounds.size.y;
        Vector3 bottomLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float offset = 0.1f;
        if (pos.y + sizeY / 2.0f + offset > bottomLeftPos.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void DestroyIfOutOfScreen()
    {
        if (!CheckIsHigherThanBottom())
        {
            Destroy(gameObject);
        }
    }

    void DetermineStartCurrentSpeed()
    {
        float curSpeed = 0;
        //ratio creating parallax (+ 1 to be able to be on depth max layer)
        float ratio = (GameManager.instance.maxDepthLayer - depthLayer + 1) / (float)GameManager.instance.maxDepthLayer;
        curSpeed = GameManager.instance.scrollingSpeed * ratio;
        SetBaseScrollingSpeed(curSpeed);
    }

    public void SetBaseScrollingSpeed(float _speed)
    {
        baseSpeed = _speed;
        UpdateScrollingSpeed();
    }

    public void SetScrollingBonusSpeed(float _bonusSpeed)
    {
        bonusSpeed = _bonusSpeed;
        UpdateScrollingSpeed();
    }
    
    void UpdateScrollingSpeed()
    {
        currentSpeed = baseSpeed + bonusSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
       //if speed hasn't already been modified (ex: boosters..)
       if(currentSpeed == 0)
       {
            DetermineStartCurrentSpeed();
       }
    }

    void UpdateScrolling()
    {
        if (isScrolling)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - currentSpeed * Time.deltaTime * GameManager.instance.timeScale, transform.position.z);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateScrollingSpeed();

        UpdateScrolling();

        DestroyIfOutOfScreen();
    }
}
