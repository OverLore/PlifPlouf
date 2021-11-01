using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateMapSelec : MonoBehaviour
{
    [SerializeField]  GameObject[] tabButtons;
    [SerializeField] GameObject statpannel;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x <= GameManager.instance.maxLevelReached + 1; x++)
        {
            tabButtons[x].SetActive(true);
        }
    }

    public void ButtonLevelEffect(int targetLevel)
    {
        GameManager.instance.levelToLoad = targetLevel;
        statpannel.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
