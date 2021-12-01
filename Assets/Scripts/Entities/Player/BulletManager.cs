using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] GameObject bulletPopVFX;

    public static BulletManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject InstantiatePlayerBulletPopVFX(Vector3 _pos)
    {
        return Instantiate(bulletPopVFX, _pos, Quaternion.identity);
    }
}
