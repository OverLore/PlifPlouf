using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitiateMapSelec : MonoBehaviour
{
    [SerializeField]  GameObject[] tabButtons;
    [SerializeField] GameObject statpannel;

    // Start is called before the first frame update
    void Start()
    {

        // for (int x = 0; x <= GameManager.instance.maxLevelReached + 1; x++)
        // {
        //     tabButtons[x].SetActive(true);
        // }

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
        for (int x = 0; x <= PlayerPrefs.GetInt(GameManager.instance.profileName + "maxLevelReached") + 1; x++)
        {
            tabButtons[x].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButtonDown("DebugUnlockNextLevel")
            && SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (GameManager.instance.maxLevelReached + 1 < 11)
            {
                GameManager.instance.ChangeMaxLevelReached(GameManager.instance.maxLevelReached + 1);
            }
            UpdateTabButtons();
        }
    }

}
