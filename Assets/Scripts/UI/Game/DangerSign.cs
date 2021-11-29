using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSign : MonoBehaviour
{
    private float timer = 0.0f;

    public void SetTimer(float _timer)
    {
        timer = _timer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime * GameManager.instance.timeScale;
        if (timer <= 0.0f)
        {
            timer = 0.0f;
            Destroy(gameObject);
        }
    }
}
