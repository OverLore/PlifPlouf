using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    [SerializeField] SpriteRenderer glowRenderer;
    Rigidbody2D rb;

    float rnd;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rnd = Random.Range(0f, 2f);
        glowRenderer.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
    }

    public void Setup(int _value)
    {
        value = _value;
    }

    private void Update()
    {
        glowRenderer.color = new Color(1, 1, 1, .3f + Mathf.Abs(Mathf.Sin(Time.time + rnd)));
        glowRenderer.transform.Rotate(new Vector3(0, 0, rnd * 20 * Time.deltaTime * GameManager.instance.timeScale));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        GameManager.instance.ChangeMoney(value);
        LevelManager.instance.coins++;
        AudioManager.Instance.PlaySound("PickUpCoin");

        PlayerPrefs.SetInt(GameManager.instance.profileName + "CoinPicked", PlayerPrefs.GetInt(GameManager.instance.profileName + "CoinPicked") + 1);

        Destroy(gameObject);
    }
}
