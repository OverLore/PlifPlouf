using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EelMove : MonoBehaviour
{
    [SerializeField] int pathChoose;
    [SerializeField] float delayMax;
    [Range(0f, 1f), SerializeField] float delayRandMax;
    [SerializeField] GameObject[] boneObject;
    public GameObject pathsObject;
    List<List<Transform>> pathsf;

    float delayNextPath = 0f;
    float delayMaxUse;
    float timerSpawn = 0f;
    float timerMax = 0.09f;
    int randomPath = 0;
    int actualBone = 0;
    int nbPaths;
    int[] nbChildPath;
    bool IsMoving = false;
    bool boneMax = false;
    Transform[] actualPath;
    Vector2[] colliderPoints;


    void ChoosePath()
    {
        if (pathChoose == -1)
        {
            randomPath = Random.Range(0, nbPaths);
        }
        else
        {
            randomPath = pathChoose;
        }

        for (int i = 0; i < boneObject.Length; i++)
        {
            boneObject[i].transform.position = pathsf[randomPath][0].GetChild(0).transform.position;
        }
    }
    void InitAllPath()
    {
        for (int i = 0; i < nbPaths; i++)
        {
            List<Transform> temp = new List<Transform>();
            temp.Add(pathsObject.transform.GetChild(i));
            pathsf.Add(temp);
            for (int j = 0; j < pathsObject.transform.GetChild(i).transform.childCount; j++)
            {
                nbChildPath[i] = pathsObject.transform.GetChild(i).transform.childCount;
                if (j == 0)
                {
                    pathsf[i][j] = (pathsObject.transform.GetChild(i).transform.GetChild(j));
                }
                else
                {
                    pathsf[i].Add(pathsObject.transform.GetChild(i).transform.GetChild(j));
                }
            }
        }
    }
    
    bool IsPathEnd()
    {
        for (int i = 0; i < boneObject.Length; i++)
        {
            int cur = 0;
            int size = 0;
            if (boneObject[i].GetComponent<SplinePathFollow>() != null)
            {
                size = boneObject[i].GetComponent<SplinePathFollow>().paths.Length - 1;
                cur = boneObject[i].GetComponent<SplinePathFollow>().currentPath;
                if (cur <= size)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    void UpdateBodyPart()
    {
        if (actualBone < boneObject.Length)
        {
            timerSpawn += Time.deltaTime * GameManager.instance.timeScale;
            if (timerSpawn >= timerMax)
            {
                SplinePathFollow pathFollow;
                if (boneObject[actualBone].GetComponent<SplinePathFollow>() == null)
                {
                    boneObject[actualBone].AddComponent<SplinePathFollow>();
                    pathFollow = boneObject[actualBone].GetComponent<SplinePathFollow>();
                    pathFollow.speed = 0.5f;
                   
                    pathFollow.correction = 90;
                    pathFollow.isDestroyedAtEnd = false;
                }
                else
                {
                    pathFollow = boneObject[actualBone].GetComponent<SplinePathFollow>();
                    pathFollow.currentPath = 0;
                }
                pathFollow.paths = new Transform[actualPath.Length];
                for (int i = 0; i < actualPath.Length; i++)
                {
                    pathFollow.paths.SetValue(actualPath[i], i);
                }
                actualBone++;
                timerSpawn -= timerMax;
            }
        }
        if (!IsPathEnd())
        {
            delayNextPath += Time.deltaTime;

            if (delayNextPath >= delayMaxUse)
            {
                delayMaxUse = delayMax + Random.Range(-delayRandMax, delayRandMax);
                actualBone = 0;
                IsMoving = false;
                delayNextPath = 0;
            }
        }
    }
    void UpdateUsePath()
    {
        actualPath = new Transform[pathsf[randomPath].Count];
        for (int i = 0; i < pathsf[randomPath].Count; i++)
        {
            actualPath[i] = pathsf[randomPath][i].transform;
            IsMoving = true;
        }
    }
    
    void UpdateCollision()
    {
        for (int i = 0; i < boneObject.Length; i++)
        {
            colliderPoints = GetComponent<EdgeCollider2D>().points;
            colliderPoints[i].x = boneObject[i].transform.position.x;
            colliderPoints[i].y = boneObject[i].transform.position.y;
            GetComponent<EdgeCollider2D>().points = colliderPoints;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        nbPaths = pathsObject.transform.childCount;
        pathsf = new List<List<Transform>>();
        nbChildPath = new int[nbPaths];
        delayMaxUse = delayMax + Random.Range(-delayRandMax, delayRandMax);
        InitAllPath();
        ChoosePath();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving)
        {
            ChoosePath();
            UpdateUsePath();
        }
        else
        {
            UpdateBodyPart();
        }
        UpdateCollision();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("oui oui baguette : " + collision);

    }
}
