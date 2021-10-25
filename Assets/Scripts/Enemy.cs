using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int PV = 1;

    [SerializeField] GameObject deathParticles;

    public void EndAnim()
    {
        Destroy(gameObject);
    }

    public void takeDamage(int dmg)
    {
        PV -= dmg;
    }

    void Update()
    {
        if (PV <= 0)
        {
            GameObject go = Instantiate(deathParticles);
            go.transform.position = transform.position;

            Destroy(go, 1f);

            Destroy(transform.parent.parent.gameObject);
        }
    }
}
