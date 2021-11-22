using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum AudioManagerSceneType
{
    Menu,
    Game
}


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    public AudioManagerSceneType sceneType = AudioManagerSceneType.Menu;

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
            Debug.LogWarning("sound : " + _name + "couldn't be found");
            return;
        }
        currentSound.source.Play();
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

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

    }

    private void Start()
    {
        LoadSoundsAtStart();
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
                _sound.source.Play();
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
