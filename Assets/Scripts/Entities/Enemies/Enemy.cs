using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float PV = 1;

    public int score;

    [SerializeField] GameObject deathParticles;

    private void Kill()
    {
        GameManager.instance.AddScore();
        LevelManager.instance.SpawnCoinAt(transform.position, score);
        BoosterManager.instance.SpawnRandomBoosterObject(transform.position);

        Destroy(transform.parent.parent.gameObject);
        //Destroy(gameObject);
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
    }
}
