using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSpeed : MonoBehaviour
{
    //depth layer will determine the scrolling speed in an idea of parallax implementation
    [SerializeField] int depthLayer;
    [SerializeField] float currentSpeed;
    public float bonusSpeed;
    bool CheckIsHigherThanBottom()
    {
        Vector3 pos = transform.position;
        Vector3 size = transform.localScale;
        Vector3 bottomLeftPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float offset = 0.1f;
        if (pos.y + size.y / 2.0f + offset > bottomLeftPos.y)
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

    float DetermineScrollingSpeed()
    {
        float result = 0;
        //ratio creating parallax (+ 1 to be able to be on depth max layer)
        float ratio = (GameManager.instance.maxDepthLayer - depthLayer + 1) / (float)GameManager.instance.maxDepthLayer;
        result = GameManager.instance.scrollingSpeed * ratio + bonusSpeed;
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = DetermineScrollingSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - currentSpeed * Time.deltaTime, transform.position.z);

        DestroyIfOutOfScreen();
    }
}
