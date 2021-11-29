using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSign : MonoBehaviour
{
    public Vector3 canvasPos = Vector3.zero;
    [SerializeField] RectTransform rt;
    [SerializeField] ParticleSystem ps;
    private ParticleSystem psChild;


    private void Start()
    {
        psChild = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    //the object will be put in world space coordinates but always according to the pos on the canvas
    //(to keep it on left or right when we move out of the screen)
    private void UpdatePos()
    {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(canvasPos);
        tempPos.z = 0;
        rt.position = tempPos;
    }

    void UpdateSimSpeed()
    {
        ParticleSystem.MainModule main = ps.main;
        main.simulationSpeed = GameManager.instance.timeScale;
        main = psChild.main;
        main.simulationSpeed = GameManager.instance.timeScale;
    }

    void UpdateDeath()
    {
        if (!ps.IsAlive() && !psChild.IsAlive())
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //changing the temp main variable manage to modify ps.main 
        UpdateSimSpeed();
        UpdatePos();
        UpdateDeath();
    }
}
