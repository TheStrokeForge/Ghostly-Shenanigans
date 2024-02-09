using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public float MasterSFX;
    //public Slider SFXSlider;

    public float MasterMusic;
    //public Slider MusicSlider;

    //public float MasterSoundFX;
    public MySound[] sounds;

    public static AudioManager instance;

    public GameObject MusicToggleButton;
    public Color MusicColor;
    public GameObject SFXToggleButton;
    public Color SFXColor;

    public Color DeactivatedColor;

    public static bool MainMenuThemePlaying = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (MySound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.PlayOnAwake;
        }

    }

    private void Start()
    {

        
        
    }

    private void Update()
    {
        //AudioSource[] AS = this.GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex < 6)
        {
            if(!MainMenuThemePlaying)
            {
                Play("MenuSound");
                StopPlaying("Spooky");
                MainMenuThemePlaying=true;
            }
            
        }
        else
        {
            if (MainMenuThemePlaying)
            {
                Play("Spooky");
                StopPlaying("MenuSound");
                MainMenuThemePlaying = false;
            }
            
        }


        foreach (AudioSource AS in this.GetComponents<AudioSource>())
        {
            if (AS.loop)
            {
                AS.volume = MasterMusic;
                //this.GetComponent<AudioSource>().volume = MasterMusic;
            }
            else
            {
                AS.volume = MasterSFX;
            }
        }



    }
    public void Play(string name)
    {
        MySound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
        }
        else
        {
            s.source.Play();
        }
        
    }

    public void StopPlaying(string name)
    {
        MySound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
        }
        s.source.Stop();
    }

    public void MusicToggle()
    {
        /*
        MySound s = Array.Find(sounds, sound => sound.name == "MenuSound");

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
        }
        if(s.source.isPlaying)
        {
            s.source.Stop();
            MusicToggleButton.GetComponent<Image>().color = DeactivatedColor;
        }
        else
        {
            s.source.Play();
            MusicToggleButton.GetComponent<Image>().color = MusicColor;

        }
        */

        if (MasterMusic == 1)
        {
            MasterMusic = 0;
            //SFXToggleButton.GetComponent<Image>().color = DeactivatedColor;
        }
        else
        {
            MasterMusic = 1;
            //SFXToggleButton.GetComponent<Image>().color = SFXColor;

        }
    }

    public void SFXToggle()
    {
        //MySound s = Array.Find(sounds, sound => sound.name == "Theme");
        //if (s == null)
        {
            //Debug.LogWarning("Sound:" + name + "not found!");
        }
        if (MasterSFX == 1)
        {
            MasterSFX = 0;
            //SFXToggleButton.GetComponent<Image>().color = DeactivatedColor;
        }
        else
        {
            MasterSFX = 1;
            //SFXToggleButton.GetComponent<Image>().color = SFXColor;

        }
    }


}
