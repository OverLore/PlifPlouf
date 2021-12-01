using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    [Range(0.1f, 3.0f)]
    public float pitch = 1.0f;

    //won't do anything if not played through "PlaySound" (so don't affect musics, play at beginning..)
    public bool hasPitchVariance = true;
    public bool loop = false;
    public bool playAtBeginning = false;
    public bool canMultiplePlay = false;

    //mixerGroup won't do anything if playInScene != Game
    public AudioManagerSceneType playInScene;

    public AudioManagerGroupType mixerGroup;

    //hide in inspector
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public AudioMixer audioMixer;


}
