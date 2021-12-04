using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Image NoMusic;
    [SerializeField] Image NoSound;

    private void Start()
    {
        UpdateMusicOffState();
        UpdateSoundOffState();
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
}
