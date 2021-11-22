using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

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
        SwitchSoundsToMenu();
    }


    //switch the sound between stopping/playing it(if it should play on Awake)
    //depending on the condition (isPlayingInMenu or !isPlayingInMenu)
    private void SwitchSound(Sound _sound, bool _boolCondition)
    {
        if (_boolCondition)
        {
            if (_sound.playAtBeginning && !_sound.source.isPlaying)
            {
                Debug.Log("play sound : " + _sound.name);
                _sound.source.Play();
            }
        }
        else
        {
            if (_sound.source.isPlaying)
            {
                Debug.Log("stop sound : " + _sound.name);
                _sound.source.Stop();
            }
        }
    }

    //stop sounds playing ingame and start playing sound playing on awake in menu
    public void SwitchSoundsToMenu()
    {
        foreach (Sound sound in sounds)
        {
            SwitchSound(sound, sound.playInMenu);
        }
    }

    //stop sounds playing inmenu and start playing sound playing on awake ingame
    public void SwitchSoundsToGame()
    {
        foreach (Sound sound in sounds)
        {
            SwitchSound(sound, sound.playInGame);
        }
    }

    private Sound GetSoundByName(string _name)
    {
        return Array.Find(sounds, sound => sound.name == _name);
    }

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
}
