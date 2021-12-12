using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMasMine : MonoBehaviour
{
    [SerializeField] GameObject flare;
    [SerializeField] List<GameObject> backObjs;
    [SerializeField] List<Color> backColors;

    float nextIn = 1f;
    bool isActive = false;

    private void Start()
    {
        if (Database.SeasonSkin == null)
        {
            isActive = false;
            flare.SetActive(false);

            foreach (GameObject go in backObjs)
            {
                go.GetComponent<SpriteRenderer>().enabled = false;
            }

            return;
        }

        isActive = Database.SeasonSkin.is_activated;

        if (!isActive)
        {
            foreach (GameObject go in backObjs)
            {
                go.GetComponent<SpriteRenderer>().enabled = false;
            }

            flare.SetActive(false);

            return;
        }

        foreach (GameObject go in backObjs)
        {
            go.GetComponent<SpriteRenderer>().enabled = true;

            go.GetComponent<SpriteRenderer>().sharedMaterial.SetColor("Backcolor", backColors[Random.Range(0, backColors.Count)]);
        }

        flare.SetActive(true);
    }

    private void Update()
    {
        if (!isActive)
        {
            this.enabled = false;

            return;
        }

        nextIn -= Time.deltaTime * GameManager.instance.timeScale;

        if (nextIn < 0)
        {
            nextIn = 1f;

            foreach (GameObject go in backObjs)
            {
                go.GetComponent<Renderer>().sharedMaterial.SetColor("Backcolor", backColors[Random.Range(0, backColors.Count)]);
            }
        }
    }
}
