using UnityEngine;

public class Entity : MonoBehaviour
{
    int m_life;
    float m_speed;
    string m_id;

    Player m_playerMovComp;

    public int life { get { return m_life; } set { m_life = value; } }
    public float speed { get { return m_speed; } set { m_speed = value; } }
    public string id { get { return m_id; } private set {} }

    public Player playerMovComp { get { return playerMovComp; } set { playerMovComp = value; } }



    Entity(int l = 0, float s = 0f, string i = "")
    {
        m_life = l;

        m_speed = s;

        m_id = i;
    }
}
