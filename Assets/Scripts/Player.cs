using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    [SerializeField] GameObject bullet;
    [SerializeField] private float shotForce = 7.5f;
    [SerializeField] private float shotSpread = 10.0f;
    float nextShot;
    [SerializeField] float delay;

    private static Vector3 lastPos = new Vector3(0.0f, 0.0f);

    #endregion

    #region Method

    #region Public
    public void TakeDamage(float _damage )
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

    /// <summary>
    /// Handle shot timer and instantiate shots
    /// </summary>
    private void HandleShot()
    {
        nextShot -= Time.deltaTime;

        Vector3 posDelta = GetDeltaMovement();
        posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;

        if (nextShot < 0.0f)
        {
            nextShot = delay;

            GameObject go = Instantiate(bullet);

            go.transform.position = transform.position;

            float angleSpread = Random.Range(-shotSpread, shotSpread);

            float velx = (posDelta.y  * shotForce) 
                * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
            float vely = (posDelta.y  * shotForce) 
                * Mathf.Sin((90.0f + angleSpread) * Mathf.Deg2Rad);

            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }

    private void Awake()
    {
        lastPos = this.gameObject.transform.position;
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
    }

    #endregion

    #endregion
}
