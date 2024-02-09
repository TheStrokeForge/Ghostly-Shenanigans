using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject[] humans;
    public GameObject ActiveHuman;
    public float JumpForce = 10f;
    public float HumanMoveSpeed = 5f;
    //public bool canJump = true;
    private Rigidbody2D humanRB;

    public GameObject Ghost;
    public float radius = 1f; // Radius for capsule toggle
    public float GhostMoveSpeed = 5f;
    public Rigidbody2D GhostRB;

    public bool GhostMode = true;

    public ParticleSystem GhostSwoosh;
    private GameObject ActiveObject;

    //public movement movementScript;

    private Vector3 GhostSpawnPosition;
    public bool isGrounded;

    GameObject ClosestHuman = null;
    public Color HighlightColor;

    float horizontalInput;
    float verticalInput;

    void Start()
    {

        GhostRB = Ghost.GetComponent<Rigidbody2D>();
        humans = GameObject.FindGameObjectsWithTag("Human");
        ActiveHuman = null;
        GhostMode = true;
    }

    GameObject FindClosestHumanObject()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        GameObject closestHuman = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject human in humans)
        {
            float distance = Vector3.Distance(currentPosition, human.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHuman = human;
            }
        }

        return closestHuman;
    }

    private void FixedUpdate()
    {
        MoveObject(horizontalInput, verticalInput);
    }
    void Update()
    {
        // Check for switch input
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleControl();
        }

        ClosestHuman = FindClosestHumanObject();

        if(GhostMode)
        {
            foreach (GameObject human in humans)
            {
                if (human == ClosestHuman)
                {
                    if (Vector2.Distance(Ghost.transform.position, human.transform.position) <= radius)
                    {
                        human.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().HighlightedMat;
                    }
                    else
                    {
                        human.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
                    }
                }
                else
                {
                    human.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
                }
            }
        }
        else
        {
            foreach (GameObject human in humans)
            { 
                human.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
            }
        }
        



        // Check for movement input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

         
       
        //Debug.Log(ActiveHuman);

        if (!GhostMode)
        {
            if(ActiveHuman.GetComponent<human>().CanJump)
            {

                if(Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
                
            }
            
        }
        if (!GhostMode)
        {
            isGrounded = ActiveHuman.GetComponent<human>().isGrounded;
        }
    }

    void MoveObject(float horizontalInput, float verticalInput)
    {

        if (GhostMode)
        {
            //handle ghost movement
            if(!FindObjectOfType<LevelManager>().LevelEnded)
            {
                GhostRB.velocity = new Vector2(horizontalInput * Time.deltaTime * GhostMoveSpeed, verticalInput * Time.fixedDeltaTime * GhostMoveSpeed);
                Flip(Ghost, horizontalInput);
            }
            else
            {
                GhostRB.velocity = new Vector2(0,0);
            }
            
        }
        else if(!GhostMode)
        {
            if (!FindObjectOfType<LevelManager>().LevelEnded)
            {
                ActiveHuman.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalInput * HumanMoveSpeed * Time.fixedDeltaTime, ActiveHuman.GetComponent<Rigidbody2D>().velocity.y);
                Flip(ActiveHuman, horizontalInput);
                Ghost.transform.position = ActiveHuman.transform.position;
                //handle movement for the active human
            }

        }
    }

    void Flip(GameObject WhatToFlip , float Direction)
    {
        if (GhostMode)
        {
            if (Direction > 0)
            {
                WhatToFlip.transform.localScale = new Vector3(1, WhatToFlip.transform.localScale.y, WhatToFlip.transform.localScale.z);
            }
            else if (Direction < 0)
            {
                WhatToFlip.transform.localScale = new Vector3(-1, WhatToFlip.transform.localScale.y, WhatToFlip.transform.localScale.z);
            }
        }
        else
        {
            if (ActiveHuman.GetComponent<human>().CanFlip)
            {
                if (Direction > 0)
                {
                    WhatToFlip.transform.localScale = new Vector3(1, WhatToFlip.transform.localScale.y, WhatToFlip.transform.localScale.z);
                }
                else if (Direction < 0)
                {
                    WhatToFlip.transform.localScale = new Vector3(-1, WhatToFlip.transform.localScale.y, WhatToFlip.transform.localScale.z);
                }
            }
        }

        
        
    }

    void Jump()
    {
        // Apply jump force only if the human is grounded
        if (isGrounded && ActiveHuman != null)
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            humanRB = ActiveHuman.GetComponent<Rigidbody2D>();
            // do this only for the active human's rigidbody
            humanRB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            //humanRB.velocity = new Vector2(humanRB.velocity.x, JumpForce);
            //canJump = false;
            ActiveHuman.GetComponent<human>().Dust.GetComponent<ParticleSystem>().Play();
            isGrounded = false;
        }
    }

    void ToggleControl()
    {
        //print("Toggle");
        if(humans.Length > 0)
        {
            if (GhostMode)
            {
                //in the humans array, check for humans which are in range, if yes, set the closest one Active Human 
                //GhostMode = false
                // Check if the capsule is within a certain radius of the circle
                //GameObject[] humans = GameObject.FindGameObjectsWithTag("Player");
                //Debug.Log("Tab is pressed");


                if (Vector2.Distance(Ghost.transform.position, ClosestHuman.transform.position) <= radius)
                {
                    // Toggle control to the human
                    FindObjectOfType<AudioManager>().Play("Swoosh");

                    ActiveHuman = ClosestHuman;
                    ClosestHuman.GetComponent<human>().isControlled = true;
                    Debug.Log(ActiveHuman);
                    Ghost.GetComponent<SpriteRenderer>().enabled = false;
                    //Ghost.SetActive(false);
                    GhostSwoosh.Play();
                    GhostMode = false;
                }


            }
            else
            {
                // leave body & get back to ghost mode 
                // GhostMode = true
                // Move the capsule to the position of the human
                FindObjectOfType<AudioManager>().Play("Swoosh");
                Ghost.transform.position = ActiveHuman.transform.position;
                //Ghost.SetActive(true);
                Ghost.GetComponent<SpriteRenderer>().enabled = true;
                ActiveHuman.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                ActiveHuman.GetComponent<human>().isControlled = false;


                // Toggle control to the capsule
                ActiveHuman = null;
                GhostSwoosh.Play();
                GhostMode = true;
            }
        }
        
    }

    
}
    