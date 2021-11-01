using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateMapSelec : MonoBehaviour
{
    [SerializeField]  GameObject[] tabButtons;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x <= GameManager.instance.maxLevelReached; x++)
        {
            tabButtons[x].SetActive(true);
        }
    }

    public void ButtonLevelEffect(int targetLevel)
    {
        //int target = 0;

        GameManager.instance.levelToLoad = targetLevel;
       // gameObject.GetInstanceID
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
