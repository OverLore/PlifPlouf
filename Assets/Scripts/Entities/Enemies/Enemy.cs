using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float PV = 1;

    public int score;

    [SerializeField] GameObject deathParticles;

    [SerializeField] Transform shotOrigin;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float shotDamage = 10.0f;
    [SerializeField] float shotSpeed = 10.0f;
    [SerializeField] bool isShootingEnemy = false;
    float shotTimer = 0.0f;
    float maxShotTimer = 2.0f;

    private void Kill()
    {
        GameManager.instance.AddScore();
        LevelManager.instance.SpawnCoinAt(transform.position, score);
        BoosterManager.instance.SpawnRandomBoosterObject(transform.position);

        if(gameObject.GetComponent<SplinePathFollow>() != null)
        {
            Destroy(transform.parent.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LevelManager.instance.maxObtainableScore += score;
    }

    public void EndAnim()
    {
        //Destroy(gameObject);
    }

    public void takeDamage(float dmg)
    {
        PV -= dmg;
    }

    void DoEnemyShot()
    {
        // create projectile
        EnemyBullet enemyBullet = Instantiate(enemyBulletPrefab).GetComponent<EnemyBullet>();
        enemyBullet.damage = shotDamage;
        // place projectile
        enemyBullet.gameObject.transform.position = shotOrigin.position;
        //get direction towards player
        Vector2 vecEP = new Vector2(GameManager.instance.player.transform.position.x - shotOrigin.position.x,
            GameManager.instance.player.transform.position.y - shotOrigin.position.y);
        float norm = Mathf.Sqrt(vecEP.x * vecEP.x + vecEP.y * vecEP.y);
        Vector2 vecEPNormalized;
        if (norm != 0)
        {
            vecEPNormalized = vecEP / norm;
        }
        else
        {
            vecEPNormalized = Vector2.zero;
        }
        //get final vec to put in rigidbody of enemyBullet
        Vector2 vecEPFinal = vecEPNormalized * shotSpeed;

        enemyBullet.GetComponent<Rigidbody2D>().velocity = vecEPFinal;
    }

    void UpdateShot()
    {
        if (isShootingEnemy)
        {
            shotTimer += Time.deltaTime;
            if (shotTimer >= maxShotTimer)
            {
                shotTimer = 0.0f;
                DoEnemyShot();
            }
        }
    }

    void Update()
    {
        if (PV <= 0)
        {
            LevelManager.instance.score += score;

            GameObject go = Instantiate(deathParticles);
            go.transform.position = transform.position;

            Destroy(go, 1f);

            Kill();
        }

        UpdateShot();
    }
}
