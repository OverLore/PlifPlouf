using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public Booster[] boostersDatabase;
    public Sprite[] backDatabase;
    public Color[] backColorsDatabase;

    public static BoosterManager instance;

    public GameObject boosterObjectPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        boostersDatabase = Resources.LoadAll<Booster>("Boosters");

        backDatabase = Resources.LoadAll<Sprite>("Sprites/Boosters/Fonds");
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

    public Sprite GetRandomBack()
    {
        return backDatabase[Random.Range(0, backDatabase.Length)];
    }

    public Color GetRandomColor()
    {
        return backColorsDatabase[Random.Range(0, backColorsDatabase.Length)];
    }

    public void SpawnRandomBoosterObject(Vector3 position)
    {
        Booster booster = boostersDatabase[Random.Range(0, boostersDatabase.Length)];

        GameObject go = Instantiate(boosterObjectPrefab);

        go.transform.position = position;

        go.GetComponent<BoosterObject>().Setup(booster);
    }
}
