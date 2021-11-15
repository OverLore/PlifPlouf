using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float PV = 1;

    public int score;

    [SerializeField] GameObject deathParticles;

    [SerializeField] Transform shotOrigin;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float shotDamage = 10.0f;
    [SerializeField] float collisionDamage = 10.0f;
    [SerializeField] float shotSpeed = 4.0f;
    [SerializeField] bool isShootingEnemy = false;
    [SerializeField] bool isInvincible = false;
    //careful, in start we add a random offset on the shotTimer at start so that each enemy will shoot at a
    //different time but at the same frequency
    float shotTimer = 0.0f;
    float maxShotTimer = 2.0f;

    private void Kill()
    {
        GameManager.instance.AddScore((uint)score);
        LevelManager.instance.SpawnCoinAt(transform.position, score);
        LevelManager.instance.kills++;

        if (Random.Range(0, 100) < 10)
        {
            BoosterManager.instance.SpawnRandomBoosterObject(transform.position);
        }

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
        //create an offset on each of them (so that enemies
        //won't shoot at the same time)
        shotTimer = Random.Range(0, maxShotTimer / 5.0f);
    }

    public void EndAnim()
    {
        //Destroy(gameObject);
    }

    public void takeDamage(float dmg)
    {
        if (!isInvincible)
        {
            PV -= dmg;
        }
    }

    void DoEnemyShot()
    {
        // create projectile
        EnemyBullet enemyBullet = Instantiate(enemyBulletPrefab).GetComponent<EnemyBullet>();
        enemyBullet.damage = shotDamage;
        // place projectile
        enemyBullet.gameObject.transform.position = shotOrigin.position;
        //get direction towards player
        Vector2 vecEP = new Vector2(GameManager.instance.player.muzzle.transform.position.x - shotOrigin.position.x,
            GameManager.instance.player.muzzle.transform.position.y - shotOrigin.position.y);
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
            shotTimer += Time.deltaTime * GameManager.instance.timeScale;
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
            //LevelManager.instance.score += score;

            GameObject go = Instantiate(deathParticles);
            go.transform.position = transform.position;

            Destroy(go, 1f);

            Kill();
        }

        UpdateShot();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(collisionDamage);
        }
    }
}
