using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MySceneManager : MonoBehaviour
{
    public static int OnGoingLevel;
    public int ongoinglevel;

    public static int OnGoingCutScene = 0;
    public int ongoingcutscene;

    public int TotalLevels = 18;

    public GameObject MainMenuPanel;
    public GameObject LevelsPanel;
    public static bool MenuSoundPlaying = false;
    public bool LevelSoundPlaying = false;

    public Color LevelCompletedColor = new Color(0.056f, 0.558f, 0.056f);
    public Color LevelActiveColor = new Color(0.7f, 0.02f, 0.07f);
    public Color LevelLockedColor = new Color(0.25f, 0.25f, 0.25f);
    public Color CutSceneColor = new Color(0.25f, 0.25f, 0.25f);

    public Material DefaultMat;
    public Material HighlightedMat;

    public GameObject SFXOnButton;
    public GameObject SFXOffButton;
    public GameObject MusicOnButton;
    public GameObject MusicOffButton;



    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            UpdateLevels();
        }
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            UpdateSoundButtons();
        }

    }

    public void UpdateSoundButtons()
    {
        if (FindObjectOfType<AudioManager>() != null)
        {
            if(FindObjectOfType<AudioManager>().MasterSFX == 1)
            {
                SFXOnButton.SetActive(true);
                SFXOffButton.SetActive(false);
            }
            else
            {
                SFXOnButton.SetActive(false);
                SFXOffButton.SetActive(true);
            }

            if (FindObjectOfType<AudioManager>().MasterMusic == 1)
            {
                MusicOnButton.SetActive(true);
                MusicOffButton.SetActive(false);
            }
            else
            {
                MusicOnButton.SetActive(false);
                MusicOffButton.SetActive(true);
            }
        }
    }
    private void Update()
    {
        ongoinglevel = OnGoingLevel;
        ongoingcutscene = OnGoingCutScene;
        //UpdateLevels();

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {   if(!MenuSoundPlaying)
            {
                //FindObjectOfType<AudioManager>().Play("MenuSound");
                MenuSoundPlaying = true;
            }
            
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }


    }

    public void UpdateLevels()
    {
        // Update Level statuses
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //MenuLevel.text = "LEVEL " + OnGoingLevel.ToString();

            if (GameObject.FindGameObjectWithTag("LevelsPanel") != null)
            {

                //Loop that goes from 1 to Level X
                int i = 1;
                while (i < TotalLevels)
                {   
                    if (LevelsPanel.transform.Find(i.ToString()) != null)
                    {   
                        print(i.ToString());
                        if (i < OnGoingLevel)
                        {
                            LevelsPanel.transform.Find(i.ToString()).gameObject.GetComponent<Image>().color = LevelCompletedColor;
                            LevelsPanel.transform.Find(i.ToString()).gameObject.GetComponent<Button>().interactable = true;
                        }
                        else if(i == OnGoingLevel)
                        {
                            LevelsPanel.transform.Find(i.ToString()).gameObject.GetComponent<Image>().color = LevelActiveColor;
                            LevelsPanel.transform.Find(i.ToString()).gameObject.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            LevelsPanel.transform.Find(i.ToString()).gameObject.GetComponent<Image>().color = LevelLockedColor;
                            LevelsPanel.transform.Find(i.ToString()).gameObject.GetComponent<Button>().interactable = false;
                        }
                    }
                    i++;
                }

                //Loop that goes from 1 to Level X
                int c = 1;
                while (c < 4)
                {
                    if (LevelsPanel.transform.Find("CutScene"+c.ToString()) != null)
                    {
                        if (c <= OnGoingCutScene)
                        {
                            LevelsPanel.transform.Find("CutScene" + c.ToString()).gameObject.GetComponent<Image>().color = CutSceneColor;
                            LevelsPanel.transform.Find("CutScene" + c.ToString()).gameObject.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            LevelsPanel.transform.Find("CutScene" + c.ToString()).gameObject.GetComponent<Image>().color = LevelLockedColor;
                            LevelsPanel.transform.Find("CutScene" + c.ToString()).gameObject.GetComponent<Button>().interactable = false;
                        }
                    }
                    c++;
                }







                //find that button in the transform, set it's status, ie, color, interactable


                //transform.Find(childObjectName);

                /*
                if (Level4)
                {
                    LevelsPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
                    LevelsPanel.transform.GetChild(4).GetComponent<Button>().interactable = true;
                }
                */
            }
        }
    }

    public void IJustCompletedALevel(int WhichOne)
    {
        if(WhichOne >= OnGoingLevel)
        {
            OnGoingLevel = WhichOne+1;
            //ongoinglevel = WhichOne;
        }
        if(WhichOne == 12)
        {
            //Hunter Cutscene
            if (OnGoingCutScene < 2)
            {
                OnGoingCutScene = 2;
            }
        }
        if (WhichOne == 16)
        {
            //Hunter Cutscene
            if (OnGoingCutScene < 3)
            {
                OnGoingCutScene = 3;
            }
        }
    }

    public void SFXToggleCaller()
    {
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().SFXToggle();
        }
    }

    public void MusicToggleCaller()
    {
        if(FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().MusicToggle();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelSelectMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelMap()
    {
        SceneManager.LoadScene(1);
        //LevelsPanel.SetActive(true);
        //MainMenuPanel.SetActive(false);
    }

    public void LoadLevel(int WhichOne)
    {
        SceneManager.LoadScene(WhichOne+5);
        if(OnGoingLevel<WhichOne)
        {
            OnGoingLevel=WhichOne;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (OnGoingLevel < (SceneManager.GetActiveScene().buildIndex -5 +1))
        {
            OnGoingLevel = SceneManager.GetActiveScene().buildIndex -5 +1;
        }
    }

    public void LoadCutScene(int WhichOne)
    {
        SceneManager.LoadScene(WhichOne+1);
        if(WhichOne>OnGoingCutScene)
        {
            OnGoingCutScene=WhichOne;
        }
    }


    public void PLAY()
    {
        //Check for Cutscenes
        if(OnGoingCutScene == 0)
        {
            LoadCutScene(1);
        }
        else
        // if No Cutscene in place then load level
        SceneManager.LoadScene(5 + OnGoingLevel + 1);


    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(5);
    }

    public void EndTut( int ToWhere)
    {
        if(OnGoingLevel == 0)
        {
            OnGoingLevel = 1;
        }

        SceneManager.LoadScene(ToWhere);
    }
    


}
