using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //public float RateOfEnemySpawn;
    //float CurrentRateOfEnemySpawn;
    //float CurrentRateOfDummySpawn;
    //public float RateOfDummySpawn;
    //public bool CanSpawn;
    //public int RemainingEnemies;

    //public float The3Secs;
    //public TextMeshProUGUI The3SecsText;
    public bool The3SecsOver = false;
    public GameObject The3secsBoard;
    public float The3SecsSpeedMultiplier;

    public GameObject WinBoard;
    public GameObject LooseBoard;

    public GameObject IntactPlayer;
    public GameObject Enemy;
    public GameObject GuardGroup;

    public bool Initialised = false;

    public bool LevelCompleted = false;
    public bool preparingForUI = false;
    public float TimBtwMatchEndAndUI;

    //public float DelayToSpawnObjects;

    //public TextMeshProUGUI DashBEnems;
    public bool LevelEnded;

    // Start is called before the first frame update
    void Start()
    {
        //The3Secs = 3;
        //RemainingEnemies = 1;
        //CurrentRateOfEnemySpawn = 0;
        //CurrentRateOfDummySpawn = 0;
        //CanSpawn = true;
        //GameObject.FindGameObjectsWithTag("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
            
        //if(!Initialised)
        //{
            //Initialise();
        //}
        

        if (preparingForUI)
        {
            TimBtwMatchEndAndUI -= Time.deltaTime;
        }

        if(TimBtwMatchEndAndUI <= 0)
        {
            preparingForUI = false;
            if(LevelCompleted)
            {
                if(!WinBoard.activeSelf)
                {
                    FindObjectOfType<AudioManager>().Play("Win");
                }
                WinBoard.SetActive(true);
            }
            else
            {
                if (!LooseBoard.activeSelf)
                {
                    FindObjectOfType<AudioManager>().Play("Lose");
                }
                //print("Lose");
                LooseBoard.SetActive(true);
                
            }

        }

        
    }

    public void WinThisLevel()
    {
        
        if (LevelEnded == false)
        {
            LevelEnded = true;
            LevelCompleted = true;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoment>().CanMove = false;
            
            FindObjectOfType<MySceneManager>().IJustCompletedALevel(SceneManager.GetActiveScene().buildIndex - 5);

            WinBoard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LEVEL "+ (SceneManager.GetActiveScene().buildIndex - 5).ToString();
            //CanSpawn = false;
            preparingForUI = true;
        }
    }

    public void LoseThisLevel(string HowWeLost)
    {
        if (LevelEnded == false)
        {
            LevelEnded = true;
            LevelCompleted = false;

            preparingForUI = true;

            LooseBoard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = HowWeLost;

            //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoment>().CanMove = false;

        }
    }

    
    public void Initialise()
    {
        Instantiate(IntactPlayer, GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform.position, Quaternion.identity);

        GameObject[] GuardPlaces = GameObject.FindGameObjectsWithTag("GuardSpawnPoint");
        if(GuardPlaces.Length > 0)
        {
            foreach (GameObject G in GuardPlaces)
            {
                Instantiate(GuardGroup, G.transform.position, G.transform.rotation);
            }
        }
        

        GameObject[] EnemyPlaces = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        
        foreach (GameObject E in EnemyPlaces)
        {
            Instantiate(Enemy, E.transform.position, E.transform.rotation);
        }

        Initialised = true;
    }
}
