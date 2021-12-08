using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSign : MonoBehaviour
{
    public Vector3 canvasPos = Vector3.zero;
    [SerializeField] RectTransform rt;
    [SerializeField] ParticleSystem ps;
    private ParticleSystem psChild;
    public bool isEelDangerSign;
    //the enemy that the danger sign target
    Transform target = null;
    public Vector2 warningSignSize = Vector2.zero;

    private void Start()
    {
        psChild = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        //horrible fix for pc version
        gameObject.transform.localScale = Vector3.one;
    }

    //do not use for eel danger sign
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    //the object will be put in world space coordinates but always according to the pos on the canvas
    //(to keep it on left or right when we move out of the screen)
    private void UpdatePos()
    {
        if (!isEelDangerSign)
        {
            if (target != null)
            {
                gameObject.transform.position = DangerSignManager.instance.GetDangerSignPosByHeadPos(target, warningSignSize);
            }
        }
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
        if (!isEelDangerSign && !ps.IsAlive() && !psChild.IsAlive())
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
