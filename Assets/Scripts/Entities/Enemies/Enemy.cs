using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float PV = 1;
    float maxPV;

    public int score;

    [SerializeField] GameObject deathParticles;

    [SerializeField] Transform shotOrigin;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float shotDamage = 10.0f;
    [SerializeField] float collisionDamage = 10.0f;
    [SerializeField] float shotSpeed = 4.0f;
    [SerializeField] bool isShootingEnemy = false;
    [SerializeField] bool isInvincible = false;
    [SerializeField] bool hasLifebar = true;
    [SerializeField] GameObject lifebarPrefab;
    [SerializeField] Vector2 lifebarOffset;
    GameObject lifebar;
    public Image lifebarImgBack;
    public Image lifebarImg;
    float lifebarVisibility = 0f;
    //careful, in start we add a random offset on the shotTimer at start so that each enemy will shoot at a
    //different time but at the same frequency
    float shotTimer = 0.0f;
    float maxShotTimer = 2.0f;

    private void Kill()
    {
        if (isInvincible)
        {
            return;
        }

        DestroyLifebar();

        AudioManager.Instance.PlaySound("DeathMob");
        GameManager.instance.AddScore((uint)score);
        LevelManager.instance.SpawnCoinAt(transform.position, score);
        LevelManager.instance.kills++;
        //Achivment stuff:
        PlayerPrefs.SetInt(GameManager.instance.profileName + "KillCount", PlayerPrefs.GetInt(GameManager.instance.profileName + "KillCount") + 1);
        //

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

    void SpawnLifebar()
    {
        lifebar = Instantiate(lifebarPrefab, GameManager.instance.lifebarCanvas.transform);
        lifebarImg = lifebar.transform.Find("Bar").GetComponent<Image>();
        lifebarImgBack = lifebar.GetComponent<Image>();

        lifebarImg.color = new Color(lifebarImg.color.r, lifebarImg.color.g, lifebarImg.color.b, lifebarVisibility);
        lifebarImgBack.color = new Color(lifebarImg.color.r, lifebarImg.color.g, lifebarImg.color.b, lifebarVisibility);
    }

    void UpdateLifebar()
    {
        RectTransform CanvasRect = GameManager.instance.lifebarCanvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        lifebar.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition + lifebarOffset;

        lifebarImg.fillAmount = PV / maxPV;

        lifebarVisibility -= Time.deltaTime * GameManager.instance.timeScale;

        lifebarVisibility = Mathf.Clamp(lifebarVisibility, 0f, 100f);

        lifebarImg.color = new Color(lifebarImg.color.r, lifebarImg.color.g, lifebarImg.color.b, lifebarVisibility);
        lifebarImgBack.color = new Color(lifebarImgBack.color.r, lifebarImgBack.color.g, lifebarImgBack.color.b, lifebarVisibility);
    }

    void DestroyLifebar()
    {
        Destroy(lifebar);
    }

    private void Start()
    {
        LevelManager.instance.maxObtainableScore += score;
        //Debug.Log(gameObject + " " + LevelManager.instance.maxObtainableScore);
        //create an offset on each of them (so that enemies
        //won't shoot at the same time)
        shotTimer = Random.Range(0, maxShotTimer / 5.0f);

        maxPV = PV;

        lifebarPrefab = Resources.Load<GameObject>("Prefabs/lifeBar");

        if (hasLifebar)
        {
            SpawnLifebar();
        }
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

            lifebarVisibility = 1f;
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
        AudioManager.Instance.PlaySound("TirEnnemi");
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
        //Debug.Log("rotation " + transform.rotation);
        if (PV <= 0 && !isInvincible)
        {
            //LevelManager.instance.score += score;
            GameObject go = Instantiate(deathParticles);
            go.transform.position = transform.position;

            Destroy(go, 1f);

            Kill();
        }

        if (hasLifebar)
        {
            UpdateLifebar();
        }
        
        if (!GameManager.instance.Paused)
        {
            UpdateShot();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(collisionDamage);
        }
    }
}
