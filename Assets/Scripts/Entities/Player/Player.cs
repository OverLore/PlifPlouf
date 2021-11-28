using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Fields
    [SerializeField]
    enum ShotType
    {
        Default,
        Triple,
        DeathRay,
        ShotAround,
        Double
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

    [SerializeField] float AttackDamageLeft = 0;
    [SerializeField] float AttackSpeedLeft = 0;
    [SerializeField] float ShieldLeft = 0;
    [SerializeField] float HorizontalShotLeft = 0;
    [SerializeField] float NumberShotLeft = 0;

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
    float lifeIndicatorTime;

    [SerializeField] Animator[] playerAnimators;
    [SerializeField] SpriteRenderer[] playerSpriteRenders;

    public int pv = 100;

    float nextShot;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float delay;
    [SerializeField] ShotType shotType = ShotType.Default;

    private static Vector3 lastPos = new Vector3(0.0f, 0.0f);

    delegate void ShotMethod();
    private Dictionary<ShotType, ShotMethod> shotMethods = new Dictionary<ShotType, ShotMethod>();

    float invincibilityTimer = 0.0f;
    [SerializeField] float maxInvincibilityTimer = 1.0f;
    bool isInvincibilityOn = false;
    [SerializeField] int invincibilityBlinkNumber = 3;


    #endregion

    #region Method

    #region Public

    public void ActivateAttackSpeed()
    {
        AttackSpeedLeft = 20f;
    }

    public void ActivateAttackDamage()
    {
        AttackDamageLeft = 20f;
    }

    public void ActivateShiel()
    {
        ShieldLeft = 20f;
    }

    public void ActivateHorizontalShot()
    {
        HorizontalShotLeft = 20f;
    }

    public void ActivateShotNumber()
    {
        NumberShotLeft = 20f;
    }

    public void TakeDamage(float _damage)
    {
        if (!isInvincibilityOn)
        {
            if (HasShield)
            {
                ShieldLeft = 0;
                Debug.Log($"Player deflected {_damage} damage");

                Shield.SetActive(false);
                AudioManager.Instance.PlaySound("MeduseGetHitShield");
                return;
            }
            else
            {
                AudioManager.Instance.PlaySound("MeduseGetHit");
            }    

            lifeIndicatorTime = 2f;

            Debug.Log($"Player take {_damage} damage");
            pv -= (int)_damage;
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
        Debug.Log(lifeTextViewportPos.y);
        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 WorldObject_ScreenPosition =  WorldObject_ScreenPosition = new Vector2(
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

        Shield.SetActive(true);

        ShieldLeft -= Time.deltaTime * GameManager.instance.timeScale;

        if (ShieldLeft <= 0)
        {
            Shield.SetActive(false);
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
        posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
        // random angle base on the spread
        float angleSpread = UnityEngine.Random.Range(-shotSpread, shotSpread);
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
            InitShotDamage(go);
            // place projectile
            go.transform.position = shotOrigin.position;
            // add push with delta position
            Vector3 posDelta = GetDeltaMovement();
            posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
            // random angle base on the spread
            float angleSpread = i * 45 + UnityEngine.Random.Range(-shotSpread / 2f, shotSpread / 2f);
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
            InitShotDamage(go);

            go.transform.parent = this.transform;
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
            // add push with delta position
            Vector3 posDelta = GetDeltaMovement();
            posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;
            // random angle base on the spread
            //float angleSpread = i * 45 + Random.Range(-shotSpread, shotSpread);

            var time = Time.realtimeSinceStartup * 10f;

            float angle = time + (float)i / 10.0f * 360.0f;

            // calculate velocity with angle
            float velx = (posDelta.y * shotForce)
                * Mathf.Cos((90.0f + angle) * Mathf.Deg2Rad);
            float vely = (posDelta.y * shotForce)
                * Mathf.Sin((90.0f + angle) * Mathf.Deg2Rad);
            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }

    private void doubleShot()
    {

        float halfSpace = 0.25f;

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

            // add push with delta position
            Vector3 posDelta = GetDeltaMovement();
            posDelta.y = posDelta.y <= 0.0f ? 1.0f : posDelta.y * 4.0f + 1.0f;

            // random angle base on the spread
            float angleSpread = UnityEngine.Random.Range(-shotSpread / 2f, shotSpread / 2f);
            // calculate velocity with angle
            float velx = (posDelta.y * shotForce)
                * Mathf.Cos((90.0f + angleSpread) * Mathf.Deg2Rad);
            float vely = (posDelta.y * shotForce)
                * Mathf.Sin((90.0f + angleSpread) * Mathf.Deg2Rad);

            // set velocity
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velx, vely);
        }
    }

    void InitShotDamage(GameObject go)
    {
        if (HasAttackDamage)
        {
            go.GetComponent<Bullet>().damage = damage * 1.5f;
        }
        else
        {
            go.GetComponent<Bullet>().damage = damage;
        }
    }

    /// <summary>
    /// Handle shot timer and instantiate shots
    /// </summary>
    private void HandleShot()
    {
        if (HasAttackSpeed)
        {
            nextShot -= Time.deltaTime * 2 * GameManager.instance.timeScale;
        }
        else
        {
            nextShot -= Time.deltaTime * GameManager.instance.timeScale;
        }

        if (nextShot < 0.0f || shotType == ShotType.DeathRay)
        {
            nextShot = delay;

            // call the current shot
            shotMethods[shotType]();
            AudioManager.Instance.PlaySound("MeduseTir");
        }
    }

    private void Awake()
    {
        lastPos = this.gameObject.transform.position;

        shotMethods[ShotType.Default] = defaultShot;
        shotMethods[ShotType.Triple] = tripleShot;
        shotMethods[ShotType.DeathRay] = deathRay;
        shotMethods[ShotType.ShotAround] = shotAround;
        shotMethods[ShotType.Double] = doubleShot;
    }

    private void UpdateLifeText()
    {
        lifeText.text = $"{pv}%";
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
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
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
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
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
            float alphaValue = Mathf.Abs(Mathf.Cos((invincibilityBlinkNumber / 2.0f) * 2 * Mathf.PI * invincibilityTimer / maxInvincibilityTimer ));
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

    // Start is called before the first frame update
    void Start()
    {
        nextShot = 0;

        Shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleShot();

        UpdateAttackDamageBoost();
        UpdateAttackSpeedBoost();
        UpdateShieldBoost();
        UpdateHorizontalShotBoost();
        UpdateShotNumberBoost();
        UpdateAnimSpeed();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
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
