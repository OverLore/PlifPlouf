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

    private void Start()
    {
        UpdateMusicOffState();
        UpdateSoundOffState();

        profileEditionWindow.SetActive(false);
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
    }

    public void PrivacyButtonEffect()
    {
        AudioManager.Instance.PlaySound("UIButton");
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
}
