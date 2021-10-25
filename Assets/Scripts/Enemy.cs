using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int PV = 1;

    public int score;

    [SerializeField] GameObject deathParticles;

    private void Kill()
    {
        GameManager.instance.AddScore();
        Destroy(gameObject);
    }

    public void EndAnim()
    {
        //Destroy(gameObject);
    }

    public void takeDamage(int dmg)
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

            //Destroy(transform.parent.parent.gameObject);
            Kill();
        }
    }
}
