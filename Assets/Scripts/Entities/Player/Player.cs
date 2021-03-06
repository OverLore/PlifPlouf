using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    #region Fields
    [SerializeField]
    enum ShotType
    {
        Default,
        Double,
        Triple,
        MultiShot,
        ShotAround
    }

    [SerializeField] GameObject Shield;

    [SerializeField] List<GameObject> shotGameobject;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    float shotForce = 7.5f;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    float shotSpread = 10.0f;

    [SerializeField]
    Transform shotOrigin;
    public Transform muzzle;

    float endTime;

    public float shieldDuration;
    private ShotType upgradeShotType;
    private float shotSideAngle;

    public float AttackDamageLeft = 0;
    public float AttackSpeedLeft = 0;
    public float ShieldLeft = 0;
    public float HorizontalShotLeft = 0;
    public float NumberShotLeft = 0;

    public bool HasAttackDamage { get { return AttackDamageLeft > 0; } }
    public bool HasAttackSpeed { get { return AttackSpeedLeft > 0; } }
    public bool HasShield { get { return ShieldLeft > 0; } }
    public bool HasHorizontalShot { get { return HorizontalShotLeft > 0; } }
    public bool HasNumberShot { get { return NumberShotLeft > 0; } }

    public bool LifeIndicatorIsActive { get { return GameManager.instance.Paused; } }

    [SerializeField]
    float damage = 1;

    [SerializeField] Canvas lifeCanvas;
    [SerializeField] RectTransform lifeRect;
    [SerializeField] RectTransform lifeComponents;
    [SerializeField] Text lifeText;
    [SerializeField] RectTransform lifeTextEnd;
    [SerializeField] Image lifeFillImage;
    [SerializeField] Image[] lifeIndicatorImages;
    [SerializeField] Text[] lifeIndicatorTexts;
    public float lifeIndicatorTime;

    [SerializeField] GameObject deathParticles;

    [SerializeField] GameObject shieldParticles;
    GameObject TestShield;

    [SerializeField] GameObject shieldEnd;
    GameObject EndShield;


    [SerializeField] Animator[] playerAnimators;
    [SerializeField] SpriteRenderer[] playerSpriteRenders;

    public int pv = 100;

    float nextShot;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float delay;
    [SerializeField] ShotType shotType;

    private static Vector3 lastPos = new Vector3(0.0f, 0.0f);

    delegate void ShotMethod();
    private Dictionary<ShotType, ShotMethod> shotMethods = new Dictionary<ShotType, ShotMethod>();

    float invincibilityTimer = 0.0f;
    [SerializeField] float maxInvincibilityTimer = 1.0f;
    bool isInvincibilityOn = false;
    [SerializeField] int invincibilityBlinkNumber = 3;

    [SerializeField] GameObject doubleDamageText;
    bool isDoubleDamage = false;

    #endregion

    #region Method

    #region Public

    public static IEnumerator MoveTowardPlayer(Vector3 _startPosition, GameObject _go, float _time)
    {
        Vector3 startingPos = _startPosition;
        Vector3 finalPos = GameManager.instance.GetPlayer().transform.Find("Muzzle").transform.position;
        float elapsedTime = 0;
        do
        {
            if (_go == null)
                yield break;
            _go.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / _time));
            elapsedTime += Time.deltaTime;
            yield return null;
        } while (elapsedTime < _time);
        //_go.transform.position = finalPos;
        //Debug.Log("Picked up !");
    }


    public void ActivateAttackSpeed()
    {
        AttackSpeedLeft = 20f;
    }

    public void ActivateAttackDamage()
    {
        AttackDamageLeft = 20f;
    }

    public void ActivateShield()
    {

        ShieldLeft = shieldDuration;

        if (TestShield == null)
        {
            TestShield = Instantiate(shieldParticles);
        }
        
    }

    public void ActivateHorizontalShot()
    {
        HorizontalShotLeft = 20f;
    }

    public void ActivateShotNumber()
    {
        NumberShotLeft = 20f;
        shotType = upgradeShotType;
    }

    public void Die()
    {
        GameObject go = Instantiate(deathParticles);
        go.transform.position = transform.position;

        Destroy(go, 1f);

        foreach (Animator anim in playerAnimators)
        {
            anim.GetComponent<SpriteRenderer>().enabled = false;
        }

        LevelManager.instance.StartScoring();
    }

    public void TakeDamage(float _damage)
    {
        if (!isInvincibilityOn && pv > 0)
        {
            if (HasShield)
            {
                AudioManager.Instance.PlaySound("MeduseGetHitShield");
                ShieldLeft = 0;
                //Debug.Log($"Player deflected {_damage} damage");
                TestShield.GetComponent<ParticleSystem>().Stop();
                TestShield.GetComponent<ParticleSystem>().Clear();
                EndShield.transform.position = gameObject.transform.position;
                EndShield.GetComponent<ParticleSystem>().Play();
                //Shield.SetActive(false);
                return;
            }
            else
            {
                AudioManager.Instance.PlaySound("MeduseGetHit");
            }

            lifeIndicatorTime = 2f;

            //Debug.Log($"Player take {_damage} damage");
            pv -= (int)_damage;

            if (pv <= 0)
            {
                Die();
                return;
            }

            SetInvincibilityOn();
        }
    }

    #endregion

    #region Private

    private void SetAnchoredPosX(float _value)
    {
        Vector3 tempPos = Vector3.zero;
        tempPos = lifeComponents.anchoredPosition;
        tempPos.x = _value;
        lifeComponents.anchoredPosition = tempPos;
    }

    private void SetAnchoredPosY(float _value)
    {
        Vector3 tempPos = Vector3.zero;
        tempPos = lifeComponents.anchoredPosition;
        tempPos.y = _value;
        lifeComponents.anchoredPosition = tempPos;
    }

    private void UpdateLifePosition()
    {
        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = lifeCanvas.GetComponent<RectTransform>();
        Vector2 lifeTextScreenPos = new Vector2(lifeTextEnd.transform.position.x, lifeTextEnd.transform.position.y);
        Vector3 lifeTextViewportPos = Camera.main.ScreenToViewportPoint(lifeTextScreenPos);
        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 WorldObject_ScreenPosition = WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        lifeRect.anchoredPosition = WorldObject_ScreenPosition;

        if (lifeTextViewportPos.x < 1.0f)
        {
            SetAnchoredPosX(0.0f);
        }
        else
        {
            SetAnchoredPosX(-500.0f);
        }

        //stop before score and combo ui
        if (lifeTextViewportPos.y < 0.86f)
        {
            SetAnchoredPosY(0.0f);
        }
        else
        {
            SetAnchoredPosY(-300.0f);
        }


    }

    void UpdateAttackSpeedBoost()
    {
        if (AttackSpeedLeft <= 0)
        {
            return;
        }

        AttackSpeedLeft -= Time.deltaTime * GameManager.instance.timeScale;
    }

    void UpdateAttackDamageBoost()
    {
        if (AttackDamageLeft <= 0)
        {
            return;
        }

        AttackDamageLeft -= Time.deltaTime * GameManager.instance.timeScale;
    }

    void UpdateShieldBoost()
    {
        if (ShieldLeft <= 0)
        {
           
            return;
            
        }

        //Shield.SetActive(true);

        ShieldLeft -= Time.deltaTime * GameManager.instance.timeScale;

        TestShield.transform.position = transform.position;


        if (ShieldLeft <= 0)
        {
            TestShield.GetComponent<ParticleSystem>().Stop();
            TestShield.GetComponent<ParticleSystem>().Clear();
            //EndShield = Instantiate(shieldEnd);
            EndShield.transform.position = gameObject.transform.position;
            EndShield.GetComponent<ParticleSystem>().Play();
            //Shield.SetActive(false);
        }
    }

    void UpdateHorizontalShotBoost()
    {
        if (HorizontalShotLeft <= 0)
        {
            return;
        }

        HorizontalShotLeft -= Time.deltaTime * GameManager.instance.timeScale;
    }

    void UpdateShotNumberBoost()
    {
        if (NumberShotLeft <= 0)
        {
            shotType = ShotType.Default;
            return;
        }

        NumberShotLeft -= Time.deltaTime * GameManager.instance.timeScale;
    }

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
        InitShotDamage(go);
        // place projectile
        go.transform.position = shotOrigin.position;
        // add push with delta position
        Vector3 posDelta = GetDeltaMovement();
        posDelta.y = 1;// posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
        // random angle base on the spread
        float angleSpread = UnityEngine.Random.Range(-shotSpread, shotSpread);
        // calculate velocity with angle
        float velx = (shotForce)
            * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
        float vely = (shotForce)
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
            InitShotDamage(go);
            // place projectile
            go.transform.position = shotOrigin.position;
            // add push with delta position
            Vector3 posDelta = GetDeltaMovement();
            posDelta.y = 1;//posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
            // random angle base on the spread
            float angleSpread = i * 35 + UnityEngine.Random.Range(-shotSpread / 2f, shotSpread / 2f);
            // calculate velocity with angle
            float velx = (shotForce)
                * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
            float vely = (shotForce)
                * Mathf.Sin((90.0f + angleSpread) * Mathf.Deg2Rad);
            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }
    private void shotAround()
    {
        for (int i = 0; i < 10; i++)
        {
            // create projectile
            GameObject go = Instantiate(shotGameobject[(int)shotType]);
            InitShotDamage(go);
            // place projectile
            go.transform.position = shotOrigin.position;
            // random angle base on the spread
            //float angleSpread = i * 45 + Random.Range(-shotSpread, shotSpread);

            var time = Time.realtimeSinceStartup * 10f;

            float angle = time + (float)i / 10.0f * 360.0f;

            // calculate velocity with angle
            float velx = (shotForce)
                * Mathf.Cos((90.0f + angle) * Mathf.Deg2Rad);
            float vely = (shotForce)
                * Mathf.Sin((90.0f + angle) * Mathf.Deg2Rad);
            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }

    private void doubleShot()
    {

        float halfSpace = 0.20f;

        for (int i = 0; i < 2; i++)
        {
            // create projectile
            GameObject go = Instantiate(shotGameobject[(int)shotType]);
            InitShotDamage(go);

            // place projectile
            if (i == 0)
            {
                go.transform.position = shotOrigin.position
                    + new Vector3(-halfSpace, 0, 0);
            }
            else
            {
                go.transform.position = shotOrigin.position
                    + new Vector3(halfSpace, 0, 0);
            }

            // random angle base on the spread
            float angleSpread = UnityEngine.Random.Range(-shotSpread / 2f, shotSpread / 2f);
            // calculate velocity with angle
            float velx = (shotForce)
                * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
            float vely = (shotForce)
                * Mathf.Sin((90.0f + angleSpread) * Mathf.Deg2Rad);

            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }


    private void SideShot()
    {
        for (int i = 0; i < 2; i++)
        {
            // create projectile
            GameObject go = Instantiate(shotGameobject[0]);
            InitShotDamage(go);

            // place projectile
            if (i == 0)
            {
                go.transform.position = shotOrigin.position;
            }
            else
            {
                go.transform.position = shotOrigin.position;
            }

            // random angle base on the spread
            float angleSpread = UnityEngine.Random.Range(-shotSpread / 2f, shotSpread / 2f);
            // calculate velocity with angle
            float velx = (shotForce)
                * Mathf.Cos((i*180.0f + angleSpread) * Mathf.Deg2Rad);
            float vely = (shotForce)
                * Mathf.Sin((i*180.0f + angleSpread) * Mathf.Deg2Rad);

            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }

    void InitShotDamage(GameObject go)
    {
        float tempDamage = 0.0f;
        if (HasAttackDamage)
        {
            tempDamage = damage * 1.5f;
        }
        else
        {
            tempDamage = damage;
        }

        if (isDoubleDamage)
        {
            tempDamage *= 2.0f;
        }

        //Debug.Log(tempDamage);
        go.GetComponent<Bullet>().damage = tempDamage;
    }

    /// <summary>
    /// Handle shot timer and instantiate shots
    /// </summary>
    private void HandleShot()
    {
        if (pv <= 0)
        {
            return;
        }

        if (HasAttackSpeed)
        {
            nextShot -= Time.deltaTime * 2 * GameManager.instance.timeScale;
        }
        else
        {
            nextShot -= Time.deltaTime * GameManager.instance.timeScale;
        }

        if (nextShot < 0.0f)
        {
            nextShot = delay;

            // call the current shot
            if (HasHorizontalShot)
            {
                SideShot();
            }
            shotMethods[shotType]();
            AudioManager.Instance.PlaySound("MeduseTir");
        }
    }

    private void Awake()
    {
        lastPos = this.gameObject.transform.position;

        shotMethods[ShotType.Default] = defaultShot;
        shotMethods[ShotType.Double] = doubleShot;
        shotMethods[ShotType.Triple] = tripleShot;
        shotMethods[ShotType.MultiShot] = () => { doubleShot(); tripleShot(); };
        shotMethods[ShotType.ShotAround] = shotAround;
    }

    private void UpdateLifeText()
    {
        //lifeText.text = $"{pv}%";
        lifeText.text = $"{pv}";
    }

    private void UpdateLifeFillImage()
    {
        lifeFillImage.fillAmount = pv / 100f;
    }

    void UpdateLifeOpacity(bool zero = false)
    {
        foreach (Image img in lifeIndicatorImages)
        {
            if (zero)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            }

            if (LifeIndicatorIsActive)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1 - (GameManager.instance.timeScale * 1.1f - .1f));
            }
            else
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Clamp01(lifeIndicatorTime));
            }
        }

        foreach (Text txt in lifeIndicatorTexts)
        {
            if (zero)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
            }

            if (LifeIndicatorIsActive)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1 - (GameManager.instance.timeScale * 1.1f - .1f));
            }
            else
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, Mathf.Clamp01(lifeIndicatorTime));
            }
        }
    }

    void UpdateAnimSpeed()
    {
        foreach (Animator a in playerAnimators)
        {
            a.speed = GameManager.instance.timeScale;
        }
    }

    void SetInvincibilityOn()
    {
        isInvincibilityOn = true;
        invincibilityTimer = 0.0f;
        //Debug.Log("set invincibility");
    }

    void SetInvincibilityOff()
    {
        isInvincibilityOn = false;
    }

    void UpdateInvincibilityTimer()
    {
        if (isInvincibilityOn)
        {
            invincibilityTimer += Time.deltaTime * GameManager.instance.timeScale;
            //Debug.Log(invincibilityTimer);
            //return a value from 0 to 1 (thus there is 2 blink during the whole invincibility,
            //thus we multiply by invincibilityBlinkNumber / 2.0f so that we get invincibilityBlinkNumber number of blink)
            float alphaValue = Mathf.Abs(Mathf.Cos((invincibilityBlinkNumber / 2.0f) * 2 * Mathf.PI * invincibilityTimer / maxInvincibilityTimer));
            foreach (SpriteRenderer sr in playerSpriteRenders)
            {
                Color currentColor = sr.color;
                currentColor.a = alphaValue;
                sr.color = currentColor;
            }
            if (invincibilityTimer >= maxInvincibilityTimer)
            {
                SetInvincibilityOff();
            }

        }
    }

    void UpdateUpgrades()
    {
        float attackSpeed = UpgradeManager.Instance.GetCurrentUpgradeByName("Attack Speed");
        delay = (1 / attackSpeed) * 10;

        float upgradeDamage = UpgradeManager.Instance.GetCurrentUpgradeByName("Attack Damage");
        damage = upgradeDamage / 10;

        float shield = UpgradeManager.Instance.GetCurrentUpgradeByName("Attack Speed");
        shieldDuration = shield;

        int type = (int)UpgradeManager.Instance.GetCurrentUpgradeByName("Shoot Number");

        switch (type)
        {
            case 1:
            {
                upgradeShotType = ShotType.Default;
                break;
            }
            case 2:
            {
                upgradeShotType = ShotType.Double;
                break;
            }
            case 3:
            {
                upgradeShotType = ShotType.Triple;
                break;
            }
            case 5:
            {
                upgradeShotType = ShotType.MultiShot;
                break;
            }
            case 10:
            {
                upgradeShotType = ShotType.ShotAround;
                break;
            }
            default:
            {
                upgradeShotType = ShotType.Default;
                break;
            }
        }

        shotSideAngle = UpgradeManager.Instance.GetCurrentUpgradeByName("Side Shot");

    }

    // Start is called before the first frame update
    void Start()
    {
        nextShot = 0;

        Shield.SetActive(false);
        EndShield = Instantiate(shieldEnd);


        UpdateUpgrades();

        doubleDamageText.SetActive(false);
    }

    public void SetDoubleDamageBool(bool _bool)
    {
        //Debug.Log("double damage set to : " + _bool);
        isDoubleDamage = _bool;
        doubleDamageText.SetActive(isDoubleDamage);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance.isEnded)
        {
            if (endTime >= 10f)
            {
                return;
            }

            endTime += Time.deltaTime * 2;

            transform.position += Vector3.up * endTime * Time.deltaTime;

            lifeIndicatorTime = 0;

            UpdateLifeOpacity();
            UpdateLifeText();
            UpdateLifeFillImage();

            //shield fx 
            if(TestShield!= null)
            {
                TestShield.GetComponent<ParticleSystem>().Stop();
                TestShield.GetComponent<ParticleSystem>().Clear();
            }

            return;
        }

        endTime = 0;

        if (!GameManager.instance.Paused)
        {
            HandleShot();
        }
        


        UpdateAttackDamageBoost();
        UpdateAttackSpeedBoost();
        UpdateShieldBoost();
        UpdateHorizontalShotBoost();
        UpdateShotNumberBoost();
        UpdateAnimSpeed();

        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButtonDown("DebugAddLife")
            && SceneManager.GetActiveScene().name == "TestNiveaux")
        {
            if (pv > 0 && pv < 100)
            {
                lifeIndicatorTime = 2f;

                pv += 10;

                if (pv > 100)
                {
                    pv = 100;
                }
            }
        }


        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButtonDown("DebugRemoveLife")
            && SceneManager.GetActiveScene().name == "TestNiveaux")
        {
            isInvincibilityOn = false;
            invincibilityTimer = 2.0f;
            TakeDamage(10);
        }


        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButtonDown("DebugDoubleDamage")
            && SceneManager.GetActiveScene().name == "TestNiveaux")
        {
            SetDoubleDamageBool(!isDoubleDamage);
        }


        UpdateLifePosition();
        if (LifeIndicatorIsActive || lifeIndicatorTime > 0)
        {
            lifeIndicatorTime -= Time.deltaTime;

            UpdateLifeOpacity();
            UpdateLifeText();
            UpdateLifeFillImage();
        }
        else
        {
            UpdateLifeOpacity(true);
        }

        UpdateInvincibilityTimer();

        // get last position
        lastPos = this.gameObject.transform.position;
    }

    #endregion

    #endregion
}
