using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int PV ;

    void Start()
    {
        PV = 2;
    }
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
            Destroy(gameObject);
        }
    }
}
