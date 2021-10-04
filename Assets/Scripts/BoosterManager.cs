using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public Booster[] boostersDatabase;

    public static BoosterManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        boostersDatabase = Resources.LoadAll<Booster>("Boosters");
    }

    public Booster GetBoosterByName(string n)
    {
        if (boostersDatabase.Length <= 0)
        {
            return null;
        }

        foreach(Booster booster in boostersDatabase)
        {
            if (booster.boosterName == n)
            {
                return Instantiate(booster);
            }
        }

        return null;
    }

    public Booster GetBoosterByRef(Booster b)
    {
        return Instantiate(b);
    }
}
