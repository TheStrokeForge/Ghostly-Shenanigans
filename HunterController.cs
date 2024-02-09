
using EZCameraShake;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class HunterController : MonoBehaviour
{
    public float biggerProximityRadius = 10f;
    public float smallerProximityRadius = 5f;
    public float RunSpeed = 3f;
    public float JumpForce = 10f;

    public bool GhostVisible = false;
    private Transform player;
    

    public Transform FrontCheck;
    public float FrontCheckRadius;
    public LayerMask GroundLayer;
    public LayerMask GhostLayer;
    public bool SomethingIsAhead = false;
    Rigidbody2D humanRB;

    public bool isPlayerAbove;
    public float playerchecklength;

    public float PlayerAboveThreshold;

    public bool IsJumping = false;

    Vector2 LookDir;
    public float angle;

    public Transform BlasterHoldPosition;
    public GameObject Blaster;
    Rigidbody2D BlasterRB;

    public GameObject KillShot;
    public GameObject Alert;
    public GameObject ZZZ;

    public bool BlasterZeroed = false;
    public float ShakeAmount;

    Vector2 MyScale;


    void Start()
    {
        player = GameObject.FindWithTag("Ghost").transform;
        humanRB = GetComponent<Rigidbody2D>();
        Blaster = Instantiate(Blaster, BlasterHoldPosition.position, Quaternion.identity);
        BlasterRB = Blaster.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!FindObjectOfType<LevelManager>().LevelEnded && !this.GetComponent<human>().IsDead)
        {
            if (GhostVisible)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);


                // Player is in Shooting range, Kill him
                if (distanceToPlayer <= smallerProximityRadius)
                {
                    BlasterRB.rotation = angle;
                    Blaster.transform.localScale = MyScale;

                    if (!FindObjectOfType<LevelManager>().LevelEnded && !this.GetComponent<human>().IsDead)
                    {
                        BlasterRB.rotation = angle;
                        Blaster.transform.localScale = MyScale;
                        Instantiate(KillShot, Blaster.transform.GetChild(0).position, Blaster.transform.rotation);
                        CameraShaker.Instance.ShakeOnce(ShakeAmount, 4f, 0.1f, 1f);
                        FindObjectOfType<AudioManager>().Play("KillShot");
                        FindObjectOfType<PlayerController>().gameObject.GetComponent<Animator>().SetBool("Ded", true);


                        //Blaster.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                        FindObjectOfType<LevelManager>().LoseThisLevel("They Got You!");
                        Debug.Log("DED!");
                    }

                }

                // Player is in chasing range, chase
                else if (distanceToPlayer <= biggerProximityRadius && distanceToPlayer > smallerProximityRadius)
                {
                    if (!this.GetComponent<human>().IsChasing)
                    {
                        Alert.GetComponent<ParticleSystem>().Play();
                    }
                    this.GetComponent<human>().IsChasing = true;

                    float XProximity = Mathf.Sign(player.position.x - transform.position.x);
                    Flip(this.gameObject, XProximity);

                    //Check if player is above
                    if (isPlayerAbove)
                    {
                        // if yes just jump
                        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
                        if (this.GetComponent<human>().isGrounded && !IsJumping)
                        {
                            Jump();
                            print("PlayerAbove");
                        }
                    }
                    else
                    {
                        // if no, check if there's obstacles in the way
                        if (SomethingIsAhead)
                        {
                            //if yes, jump
                            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
                            if (this.GetComponent<human>().isGrounded && !IsJumping)
                            {
                                Jump();
                                print("kuch aage");
                            }
                        }
                        else
                        {
                            //if no, move ahead
                            this.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(XProximity) * RunSpeed * Time.deltaTime, this.GetComponent<Rigidbody2D>().velocity.y);
                        }
                    }
                }
                else
                {
                    if (this.GetComponent<human>().IsChasing)
                    {
                        ZZZ.GetComponent<ParticleSystem>().Play();
                    }

                    //print("ChaseStop");
                    this.GetComponent<human>().IsChasing = false;
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            else
            {
                if (this.GetComponent<human>().IsChasing)
                {
                    ZZZ.GetComponent<ParticleSystem>().Play();
                }
                this.GetComponent<human>().IsChasing = false;
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
            }
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            this.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
    void Update()
    {
        //Control Blaster
        MyScale = transform.localScale;
        if (!this.GetComponent<human>().IsDead)
        {
            Blaster.transform.position = BlasterHoldPosition.position;
        }
        

        LookDir = new Vector2
        (
            player.position.x - transform.position.x,
            player.position.y - transform.position.y

        ).normalized;

        angle = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg;
        if (angle <= 90f && angle > -90f)
        {
            MyScale = new Vector3(1, 1, 1);
        }
        else
        {
            MyScale = new Vector3(1, -1, 1);
        }


        float distance = Vector2.Distance(transform.position, player.position);

        if(distance <= biggerProximityRadius && distance >= smallerProximityRadius && GhostVisible)
        {
            BlasterRB.rotation = angle;
            Blaster.transform.localScale = MyScale;
        }
        else if(distance < smallerProximityRadius)
        {
            BlasterRB.rotation = angle;
            Blaster.transform.localScale = MyScale;
        }
        else
        {
            BlasterRB.rotation = 0;
            Blaster.transform.localScale = new Vector2(this.transform.localScale.x,1);
        }
        




        //Control Hunter
        GhostVisible = player.GetComponent<SpriteRenderer>().enabled == true;

        SomethingIsAhead = CheckAhead();

        if(Mathf.Abs(transform.position.x - player.transform.position.x) < PlayerAboveThreshold)
        {
            isPlayerAbove = true;
        }
        else
        {
            isPlayerAbove = false;
        }

        
        
        
    }

    bool CheckAhead()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(FrontCheck.position, FrontCheckRadius, GroundLayer);

        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    return true; // At least one collider (other than self) is touching the ground
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    void Flip(GameObject WhatToFlip, float Direction)
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

    void Jump()
    {
        //GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //humanRB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        humanRB.velocity = new Vector2(humanRB.velocity.x, JumpForce);
        //humanRB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        this.GetComponent<human>().isGrounded = false;
        //this.GetComponent<human>().Dust.GetComponent<ParticleSystem>().Play();
        FindObjectOfType<AudioManager>().Play("Jump");
        //canJump = false;
        IsJumping = true;
        //isJumping = true;
    }

    
}