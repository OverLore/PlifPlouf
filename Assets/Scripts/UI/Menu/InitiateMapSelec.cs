using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitiateMapSelec : MonoBehaviour
{
    [SerializeField]  GameObject[] tabButtons;
    [SerializeField] GameObject statpannel;

    [SerializeField] Sprite LockedSprite;
    [SerializeField] Sprite UnLockedSprite;
    [SerializeField] Sprite ThreeStarSprite;


    LevelDatasStruct DataLevelTest;


    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < tabButtons.Length ; x++)
        {
            tabButtons[x].gameObject.GetComponent<Button>().enabled = false;
        }
            UpdateTabButtons();

    }

    public void ButtonLevelEffect(int targetLevel)
    {
        //audio
        AudioManager.Instance.PlaySound("UIButton");

        GameManager.instance.levelToLoad = targetLevel;
        statpannel.gameObject.SetActive(true);

    }
    public void ButtonOffStatEffect()
    {
        statpannel.gameObject.SetActive(false);
    }

    void UpdateTabButtons()
    {
        for (int x = 0; x <= PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached")  ; x++)
        {
            


            tabButtons[x].gameObject.GetComponent<Button>().enabled = true;
            tabButtons[x].gameObject.GetComponent<Image>().sprite = UnLockedSprite;

            LevelDatas.LoadLevelDatas(x, out DataLevelTest);
            if (DataLevelTest.stars == 3)
            {
                tabButtons[x].gameObject.GetComponent<Image>().sprite = ThreeStarSprite;
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButtonDown("DebugUnlockNextLevel")
            && SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (GameManager.instance.maxLevelReached  < 9)
            //if (PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached" ) < 9)
            {
                GameManager.instance.ChangeMaxLevelReached(GameManager.instance.maxLevelReached + 1);
            }
           
        }
        UpdateTabButtons();
    }

}
