using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Image NoMusic;
    [SerializeField] Image NoSound;
    [SerializeField] TMP_InputField newUserField;
    [SerializeField] TMP_Text currentUserText;
    [SerializeField] GameObject profileEditionWindow;
    [SerializeField] GameObject credits;

    private void Start()
    {
        UpdateMusicOffState();
        UpdateSoundOffState();

        profileEditionWindow.SetActive(false);
        credits.SetActive(false);
    }

    private void Update()
    {
        currentUserText.text = GameManager.instance.profileName;
    }

    public void SwitchMusic()
    {
        AudioManager.Instance.PlaySound("UIButton");
        AudioManager.Instance.SwitchMusicVolumeState();
        UpdateMusicOffState();
    }

    public void SwitchSound()
    {
        AudioManager.Instance.PlaySound("UIButton");
        AudioManager.Instance.SwitchSoundVolumeState();
        UpdateSoundOffState();
    }

    public void TermsButtonEffect()
    {
        AudioManager.Instance.PlaySound("UIButton");

        Application.OpenURL("https://lucarnould08.wixsite.com/overlore-studios/terms");
    }

    public void PrivacyButtonEffect()
    {
        AudioManager.Instance.PlaySound("UIButton");

        Application.OpenURL("https://lucarnould08.wixsite.com/overlore-studios/privacy");
    }

    public void ContactButtonEffect()
    {
        AudioManager.Instance.PlaySound("UIButton");
    }

    void UpdateMusicOffState()
    {
        NoMusic.enabled = AudioManager.Instance.canHearMusic ? false : true;
    }

    void UpdateSoundOffState()
    {
        NoSound.enabled = AudioManager.Instance.canHearSound ? false : true;
    }

    public void OpenProfileEditionWindow()
    {
        AudioManager.Instance.PlaySound("UIButton");
        profileEditionWindow.SetActive(true);
    }

    public void CloseProfileEditionWindow()
    {
        AudioManager.Instance.PlaySound("UIButton");
        profileEditionWindow.SetActive(false);
    }

    public void OnNewUserFieldValueChanged(string val)
    {
        AudioManager.Instance.PlaySound("UIButton");
        newUserField.text = val.ToLower();
        newUserField.text = Regex.Replace(newUserField.text, @"[^a-z0-9 ]", "");
    }

    public void OpenCreditsScreen()
    {
        AudioManager.Instance.PlaySound("UIButton");

        credits.SetActive(true);
    }

    public void CloseCreditsScreen()
    {
        AudioManager.Instance.PlaySound("UIButton");

        credits.SetActive(false);
    }
}
