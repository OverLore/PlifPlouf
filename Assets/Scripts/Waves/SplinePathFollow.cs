using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePathFollow : MonoBehaviour
{
    public bool isDestroyedAtEnd = true;
    public Transform[] paths;

    int currentPath;

    float tParam;
    public float speed = .5f;
    public float correction = 0;

    Vector2 position;
    Vector2 lastPos;

    bool useCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        currentPath = 0;
        tParam = 0;
        useCoroutine = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (paths[0] != null && useCoroutine && currentPath <= paths.Length - 1)
        {
            StartCoroutine(GoByPath(currentPath));
        }
    }

    IEnumerator GoByPath(int pathID)
    {
        useCoroutine = false;

        Vector2 p0 = paths[pathID].GetChild(0).position;
        Vector2 p1 = paths[pathID].GetChild(1).position;
        Vector2 p2 = paths[pathID].GetChild(2).position;
        Vector2 p3 = paths[pathID].GetChild(3).position;
    
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speed;

            position = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            Vector3 diff = position - lastPos;

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90 + correction);

            transform.position = position;

            lastPos = position;

            yield return new WaitForEndOfFrame();
        }

        tParam = 0;

        currentPath++;

        if (isDestroyedAtEnd)
        {
            if (currentPath > paths.Length - 1)
            {
                Destroy(transform.parent.parent.gameObject);
            }
        }

        useCoroutine = true;
    }
}
