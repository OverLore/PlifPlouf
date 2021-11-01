using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EelMove : MonoBehaviour
{
    [SerializeField] float timerMax;
    [SerializeField] GameObject[] boneObject;
    public Transform[] paths;
    float timerSpawn = 0f;
    int actualBone = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (actualBone < boneObject.Length)
        {
            timerSpawn += Time.deltaTime;
            if (timerSpawn >= timerMax)
            {
                boneObject[actualBone].AddComponent<SplinePathFollow>();
                SplinePathFollow pathFollow = boneObject[actualBone].GetComponent<SplinePathFollow>();
                pathFollow.speed = 0.5f;
                pathFollow.paths = new Transform[paths.Length];
                for (int i = 0; i < paths.Length; i++)
                {
                    pathFollow.paths.SetValue(paths[i], i);
                }
                pathFollow.correction = 90;
                pathFollow.isDestroyedAtEnd = false;

                actualBone++;
                timerSpawn -= timerMax;
            }
        }      
    }
}
