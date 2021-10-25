using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateMapSelec : MonoBehaviour
{
    [SerializeField]  GameObject[] tabButtons;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < GameManager.instance.maxLevelReached; )
        {
            tabButtons[x].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
