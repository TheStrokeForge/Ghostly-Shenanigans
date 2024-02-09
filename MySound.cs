using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class MySound
{
    public string name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool PlayOnAwake;

    public bool loop;

    public bool IsSong;



    [HideInInspector]
    public AudioSource source;
}
