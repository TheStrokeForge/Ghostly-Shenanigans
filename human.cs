using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class human : MonoBehaviour
{
    public bool isControlled;
    public bool isGrounded;
    public Transform groundCheck1;
    public Transform groundCheck2;

    bool Grounded1 = false;
    bool Grounded2 = false;
    public bool CanJump = true;
    public bool CanFlip = true;

    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    
    public bool IsHunter = false;
    public bool IsChasing = false;
    // Start is called before the first frame update

    public GameObject DeathParticles;
    public GameObject Dust;
    public GameObject WaterSplashParticles;

    public Animator animator;
    public bool IsDead = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        if(this.GetComponent<HunterController>() != null )
        {
            IsHunter = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            if(Grounded1 && Grounded2)
            {
                Dust.GetComponent<ParticleSystem>().Play();
                isGrounded = true;
            }
        }


        isGrounded = Grounded1 || Grounded2;
        

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }

        Grounded1 = CheckGround(groundCheck1);
        Grounded2 = CheckGround(groundCheck2);

        if (animator != null)
        {
            animator.SetBool("IsJumping", !isGrounded);
        }

        if (this.GetComponent<HunterController>() != null)
        {
            this.GetComponent<HunterController>().IsJumping = !isGrounded;
        }

        //if (Physics2D.Raycast(groundCheck.position,Direction,out hit,groundCheckRadius))
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        
        if (!isControlled && !IsChasing)
        {
            //this.gameObject.layer = PassiveHuman;
            if (isGrounded)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                    
            }
            else
            {
                rb.isKinematic = false;
            }
        }
        else
        {
            rb.isKinematic = false;
            //this.gameObject.layer = ActiveHuman;
        }
        
    }
    bool CheckGround(Transform Pos)
    {
        // Perform the overlap circle check
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Pos.position, groundCheckRadius, groundLayer);

        // Ignore self collider if there are multiple colliders (e.g., attached to the same GameObject)
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true; // At least one collider (other than self) is touching the ground
            }
        }

        return false; // No colliders (other than self) are touching the ground
    }
    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("Splash");
        FindObjectOfType<AudioManager>().Play("Bubble");

        IsDead = true;

        Instantiate(DeathParticles, this.transform.GetChild(0).position, Quaternion.identity);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<SpriteRenderer>().enabled = false;

        if(!IsHunter)
        {
            FindObjectOfType<LevelManager>().LoseThisLevel("Shame On You!");
        }
        else
        {
            this.GetComponent<HunterController>().Blaster.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.SetActive(false);
        }
    }
    public void WaterSplash()
    {
        Instantiate(WaterSplashParticles, this.transform.GetChild(0).position, Quaternion.identity);
    }

    public void PlayStepSound()
    {
        FindObjectOfType<AudioManager>().Play("Step");
    }
}