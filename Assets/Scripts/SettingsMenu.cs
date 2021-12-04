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
    [SerializeField] GameObject profileEditionWindow;

    private void Start()
    {
        UpdateMusicOffState();
        UpdateSoundOffState();

        profileEditionWindow.SetActive(false);
    }

    public void SwitchMusic()
    {
        AudioManager.Instance.SwitchMusicVolumeState();
        UpdateMusicOffState();
    }

    public void SwitchSound()
    {
        AudioManager.Instance.SwitchSoundVolumeState();
        UpdateSoundOffState();
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
        profileEditionWindow.SetActive(true);
    }

    public void CloseProfileEditionWindow()
    {
        profileEditionWindow.SetActive(false);
    }

    public void OnNewUserFieldValueChanged(string val)
    {
        newUserField.text = val.ToLower();
        newUserField.text = Regex.Replace(newUserField.text, @"[^a-z0-9 ]", "");
    }
}
