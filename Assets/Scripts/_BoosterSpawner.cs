using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BoosterSpawner : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
                position = new Vector3(position.x, position.y, 0);

                BoosterManager.instance.SpawnRandomBoosterObject(position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position = new Vector3(position.x, position.y, 0);

            BoosterManager.instance.SpawnRandomBoosterObject(position);
        }
    }
}
