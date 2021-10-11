using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    #region Fields
    [SerializeField]
    enum ShotType
    {
        Default,
        Triple,
        DeathRay
    }


    [SerializeField] List<GameObject> shotGameobject;
    [SerializeField] float shotForce = 7.5f;
    [SerializeField] float shotSpread = 10.0f;
    float nextShot;
    [SerializeField] float delay;
    [SerializeField] ShotType shotType = ShotType.Default;

    private static Vector3 lastPos = new Vector3(0.0f, 0.0f);

    delegate void ShotMethod();
    private Dictionary<ShotType, ShotMethod> shotMethods = new Dictionary<ShotType, ShotMethod>();

    #endregion

    #region Method

    #region Public
    public void TakeDamage(float _damage)
    {
        Debug.Log($"Player take {_damage} damage");
    }

    #endregion

    #region Private

    private Vector3 GetDeltaMovement()
    {
        var delta = this.gameObject.transform.position - lastPos;
        lastPos = this.gameObject.transform.position;

        //Debug.Log($"Player delta pos : {delta}");
        return delta;
    }

    private void defaultShot()
    {
        // create projectile
        GameObject go = Instantiate(shotGameobject[(int)shotType]);
        // place projectile
        go.transform.position = transform.position;
        // add push with delta position
        Vector3 posDelta = GetDeltaMovement();
        posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
        // random angle base on the spread
        float angleSpread = Random.Range(-shotSpread, shotSpread);
        // calculate velocity with angle
        float velx = (posDelta.y * shotForce)
            * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
        float vely = (posDelta.y * shotForce)
            * Mathf.Sin((90.0f + angleSpread) * Mathf.Deg2Rad);
        // set velocity
        go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
    }

    private void tripleShot()
    {
        for (int i = -1; i < 2; i++)
        {
            // create projectile
            GameObject go = Instantiate(shotGameobject[(int)shotType]);
            // place projectile
            go.transform.position = transform.position;
            // add push with delta position
            Vector3 posDelta = GetDeltaMovement();
            posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
            // random angle base on the spread
            float angleSpread = i * 45 + Random.Range(-shotSpread, shotSpread);
            // calculate velocity with angle
            float velx = (posDelta.y * shotForce)
                * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
            float vely = (posDelta.y * shotForce)
                * Mathf.Sin((90.0f + angleSpread) * Mathf.Deg2Rad);
            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }

    private void deathRay()
    {
        if (GetComponentInChildren<DeathRay>() == null)
        {
            GameObject go = Instantiate(shotGameobject[(int)shotType]);

            go.transform.parent = this.transform;
        }
    }

    /// <summary>
    /// Handle shot timer and instantiate shots
    /// </summary>
    private void HandleShot()
    {
        nextShot -= Time.deltaTime;

        if (nextShot < 0.0f || shotType == ShotType.DeathRay)
        {
            nextShot = delay;

            // call the current shot
            shotMethods[shotType]();
        }
    }

    private void Awake()
    {
        lastPos = this.gameObject.transform.position;

        shotMethods[ShotType.Default] = defaultShot;
        shotMethods[ShotType.Triple] = tripleShot;
        shotMethods[ShotType.DeathRay] = deathRay;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleShot();

        // get last position
        lastPos = this.gameObject.transform.position;
    }

    #endregion

    #endregion
}
