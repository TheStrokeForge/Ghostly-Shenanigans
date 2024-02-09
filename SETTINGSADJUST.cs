using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SETTINGSADJUST : MonoBehaviour
{
    public static float Mus = 1;
    public static float SFX = 1;

    //public Slider Mus;
    //public Slider SFX;

    private void Start()
    {

        if (transform.GetChild(0) != null)
        {
            transform.GetChild(0).GetComponent<Slider>().value = FindObjectOfType<AudioManager>().MasterMusic;
        }

        if (transform.GetChild(1) != null)
        {
            transform.GetChild(1).GetComponent<Slider>().value = FindObjectOfType<AudioManager>().MasterSFX;
        }
    }

    private void Update()
    {
        

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //FindObjectOfType<AudioManager>().MusicSlider = transform.GetChild(0).GetComponent<Slider>();

            //FindObjectOfType<AudioManager>().SFXSlider = transform.GetChild(1).GetComponent<Slider>();

            FindObjectOfType<AudioManager>().MasterMusic = Mus;
            FindObjectOfType<AudioManager>().MasterSFX = SFX;

            
        }
        
        
    }

    public void SetMasterMus(float HowMuch)
    {
        Mus = HowMuch;
    }

    public void SetMasterSFX(float HowMuch)
    {
        SFX = HowMuch;
    }








}
