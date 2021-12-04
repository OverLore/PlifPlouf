using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public enum AudioManagerSceneType
{
    Menu,
    Game
}

public enum AudioManagerGroupType
{
    SFXNormal,
    SFXUnderwater,
    Music
}

public class AudioManager : MonoBehaviour
{
    public AudioManagerSceneType sceneType = AudioManagerSceneType.Menu;
    [SerializeField] AudioMixerGroup gameMixerGroup;
    [SerializeField] AudioMixerGroup menuMixerGroup;
    [SerializeField] AudioMixerGroup musicMixerGroup;
    [SerializeField] AudioMixerGroup sfxMixerGroup;
    [SerializeField] AudioMixerGroup sfxNormalMixerGroup;
    [SerializeField] AudioMixerGroup sfxUnderwaterMixerGroup;
    [SerializeField] AudioMixerGroup menuMusicMixerGroup;
    [SerializeField] AudioMixerGroup menuSFXMixerGroup;

    [SerializeField] AudioMixer mixer;

    public Sound[] sounds;
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public bool canHearMusic = true;
    public bool canHearSound = true;

    #region public
    //add additional scenes here if we have a real scene transition in game
    //(if you just want sounds to play in your new scene, simply add the audioManager
    //and set the sceneType parameter to the corresponding type of your scene)
    //should only be played in sceneLoader.LoadScene
    public void LoadSoundsFromSceneName(string sceneString)
    {
        switch (sceneString)
        {
            case "MainMenu":
                SwitchSoundsToMenu();
                break;

            case "TestNiveaux":
                SwitchSoundsToGame();
                break;

            default:
                break;
        }
    }

    //play/pause/stop sound at any moment using these methods
    public void PlaySound(string _name)
    {
        Sound currentSound = GetSoundByName(_name);
        if (currentSound == null)
        {
            Debug.LogError("sound : " + _name + " couldn't be found");
            return;
        }

        //change pitch
        if (currentSound.hasPitchVariance)
        {
            currentSound.source.pitch = currentSound.pitch + UnityEngine.Random.Range(-0.1f, 0.1f);
        }

        if (currentSound.canMultiplePlay)
        {
            //Debug.Log("play multiple");
            currentSound.source.PlayOneShot(currentSound.source.clip);
        }
        else
        {
            currentSound.source.Play();
        }
    }

    public void PauseSound(string _name)
    {
        Sound currentSound = GetSoundByName(_name);
        if (currentSound == null)
        {
            Debug.LogWarning("sound : " + _name + "couldn't be found");
            return;
        }
        currentSound.source.Pause();
    }

    public void StopSound(string _name)
    {
        Sound currentSound = GetSoundByName(_name);
        if (currentSound == null)
        {
            Debug.LogWarning("sound : " + _name + "couldn't be found");
            return;
        }
        currentSound.source.Stop();
    }

    public void SwitchMusicVolumeState()
    {
        canHearMusic = !canHearMusic;

        PlayerPrefs.SetString("canHearMusic", canHearMusic.ToString());

        mixer.SetFloat("MusicVolume", canHearMusic ? -12 : -80);
        mixer.SetFloat("MenuMusicVolume", canHearMusic ? -12 : -80);
    }

    public void SwitchSoundVolumeState()
    {
        canHearSound = !canHearSound;

        PlayerPrefs.SetString("canHearSound", canHearSound.ToString());

        mixer.SetFloat("GameVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("SFXVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("SFXNormalVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("SFXUnderwaterVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("MenuSFXVolume", canHearSound ? 0 : -80);
    }
    #endregion


    #region private

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //sounds
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            switch (sound.playInScene)
            {
                case AudioManagerSceneType.Menu:
                    //sound.source.outputAudioMixerGroup = menuMixerGroup;
                    switch (sound.mixerGroup)
                    {
                        case AudioManagerGroupType.Music:
                            sound.source.outputAudioMixerGroup = menuMusicMixerGroup;
                            break;
                        case AudioManagerGroupType.SFXNormal:
                            sound.source.outputAudioMixerGroup = menuSFXMixerGroup;
                            break;

                        case AudioManagerGroupType.SFXUnderwater:
                            //redirect on menuSFX anyway
                            sound.source.outputAudioMixerGroup = menuSFXMixerGroup;
                            break;
                        default:
                            Debug.LogError("audioMixer of sound " + sound + "is invalid");
                            sound.source.outputAudioMixerGroup = null;
                            break;
                    }
                    break;

                case AudioManagerSceneType.Game:
                    switch (sound.mixerGroup)
                    {
                        case AudioManagerGroupType.Music:
                            sound.source.outputAudioMixerGroup = musicMixerGroup;
                            break;
                        case AudioManagerGroupType.SFXNormal:
                            sound.source.outputAudioMixerGroup = sfxNormalMixerGroup;
                            break;

                        case AudioManagerGroupType.SFXUnderwater:
                            sound.source.outputAudioMixerGroup = sfxUnderwaterMixerGroup;
                            break;
                        default:
                            Debug.LogError("audioMixer of sound " + sound + "is invalid");
                            sound.source.outputAudioMixerGroup = null;
                            break;
                    }


                    break;

                default:
                    break;
            }
        }

        LoadSavedStates();
    }

    void LoadSavedStates()
    {
        if (PlayerPrefs.HasKey("canHearMusic"))
        {
            canHearMusic = bool.Parse(PlayerPrefs.GetString("canHearMusic"));
        }
        else
        {
            canHearMusic = true;
            PlayerPrefs.SetString("canHearMusic", true.ToString());
        }

        if (PlayerPrefs.HasKey("canHearSound"))
        {
            canHearSound = bool.Parse(PlayerPrefs.GetString("canHearSound"));
        }
        else
        {
            canHearSound = true;
            PlayerPrefs.SetString("canHearSound", true.ToString());
        }
    }

    private void Start()
    {
        LoadSoundsAtStart();
        StartSoundVolumes();
    }

    void StartSoundVolumes()
    {
        mixer.SetFloat("MusicVolume", canHearMusic ? -12 : -80);

        mixer.SetFloat("GameVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("SFXVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("SFXNormalVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("SFXUnderwaterVolume", canHearSound ? 0 : -80);
        mixer.SetFloat("MenuVolume", canHearSound ? 0 : -80);
    }

    private void LoadSoundsAtStart()
    {
        switch (sceneType)
        {
            case AudioManagerSceneType.Menu:
                SwitchSoundsToMenu();
                break;

            case AudioManagerSceneType.Game:
                SwitchSoundsToGame();
                break;

            default:
                Debug.Log("wrong AudioManagerSceneType : " + sceneType);
                break;
        }
    }


    //switch the sound between stopping/playing it(if it should play on Awake)
    //depending on the condition (isPlayingInMenu or !isPlayingInMenu)
    private void SwitchSound(Sound _sound, bool _boolCondition)
    {
        if (_boolCondition)
        {
            if (_sound.playAtBeginning && !_sound.source.isPlaying)
            {
                //Debug.Log("play sound : " + _sound.name);
                if (_sound.canMultiplePlay)
                {
                    //Debug.Log("play multiple at start");
                    _sound.source.PlayOneShot(_sound.source.clip);
                }
                else
                {
                    _sound.source.Play();
                }
            }
        }
        else
        {
            if (_sound.source.isPlaying)
            {
                //Debug.Log("stop sound : " + _sound.name);
                _sound.source.Stop();
            }
        }
    }

    //stop sounds playing ingame and start playing sound playing on awake in menu
    private void SwitchSoundsToMenu()
    {
        foreach (Sound sound in sounds)
        {
            SwitchSound(sound, sound.playInScene == AudioManagerSceneType.Menu);
        }
    }

    //stop sounds playing inmenu and start playing sound playing on awake ingame
    private void SwitchSoundsToGame()
    {
        foreach (Sound sound in sounds)
        {
            SwitchSound(sound, sound.playInScene == AudioManagerSceneType.Game);
        }
    }

    private Sound GetSoundByName(string _name)
    {
        return Array.Find(sounds, sound => sound.name == _name);
    }
    #endregion




    
}
