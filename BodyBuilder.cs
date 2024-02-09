using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBuilder : MonoBehaviour
{
    public Transform RayPosition, GrabPosition;
    public float rayDistance;
    public GameObject grabbedObject;
    public LayerMask layerIndex;
    //public float speed = 5f; // Speed of the character movement
    public PlayerController ghostmovement1;
    public bool BoulderInRange = false;

    GameObject HighlightedBoulder = null;

    //public BoxCollider2D BoulderCollider;

    //public bool IsLifting = false;

    void Start()
    {
        //layerIndex = LayerMask.NameToLayer("Rock");
    }

    void Update()
    {
        // Handle movement
        //Move();

        
        
        if(grabbedObject != null)
        {
            //grabbedObject.transform.position = GrabPosition.position;
            //grabbedObject.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity;
            grabbedObject.transform.position = new Vector2(GrabPosition.position.x, grabbedObject.transform.position.y);

        }

        RaycastHit2D hitInfo = Physics2D.Raycast(RayPosition.position, Mathf.Sign(transform.localScale.x)*transform.right, rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Boulder")
        {
            BoulderInRange = true;
            HighlightedBoulder = hitInfo.collider.gameObject;
         
            hitInfo.collider.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().HighlightedMat;
            
            

            if (Input.GetKeyDown(KeyCode.E) && FindObjectOfType<PlayerController>().ActiveHuman == this.gameObject) 
            {
                print("PressE");
                if (grabbedObject == null)
                {
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Boulder>().enabled = false;

                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    //grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 0;

                    grabbedObject.transform.position = new Vector2(GrabPosition.position.x, grabbedObject.transform.position.y);
                    //grabbedObject.transform.SetParent(GrabPosition);
                    this.GetComponent<human>().CanJump = false;
                    this.GetComponent<human>().CanFlip = false;


                    //BoulderCollider.enabled = true;
                }
                else
                {
                    grabbedObject.GetComponent<Boulder>().enabled = true;
                    //grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    grabbedObject = null;
                    this.GetComponent<human>().CanJump = true;
                    this.GetComponent<human>().CanFlip = true;
                    //grabbedObject.transform.parent = null;
                    
                    
                }

            }
        }
        else
        {
            BoulderInRange = false;
            if(HighlightedBoulder != null)
            {
                HighlightedBoulder.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
                HighlightedBoulder = null;
            }
            if(grabbedObject != null)
            {
                grabbedObject.GetComponent<Boulder>().enabled = true;
                grabbedObject = null;
                this.GetComponent<human>().CanJump = true;
                this.GetComponent<human>().CanFlip = true;
            }



            this.GetComponent<human>().CanJump = true;
            this.GetComponent<human>().CanFlip = true;
            //grabbedObject.transform.parent = null;
        }
        // Debug.DrawRay(RayPosition.position, transform.right * rayDistance);
    }

    //void Move()
    //{
    //    // Get horizontal input axis value
    //    float horizontalInput = Input.GetAxis("Horizontal");

    //    // Calculate movement direction based on input axis
    //    Vector2 movement = new Vector2(horizontalInput, 0f);

    //    // Move the character using Rigidbody2D
    //    GetComponent<Rigidbody2D>().velocity = movement * speed;
    //}
}
