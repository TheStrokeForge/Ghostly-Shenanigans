using UnityEngine;

public class ColorChangeOnTrigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer1;
    //public string currentcolor;

    public Sprite Pressed;
    public Sprite Released;

    //public AudioClip ToggleSound;
    public int PplOnButton = 0;
    public bool IsPressed = false;


    private void FixedUpdate()
    {
        if(PplOnButton <= 0)
        {
            spriteRenderer1.sprite = Released;
        }
        else
        {
            spriteRenderer1.sprite = Pressed;
        }

        //
    }
    void Start()
    {
        // Get the SpriteRenderer component attached to the rectangle GameObject
        spriteRenderer1 = GetComponent<SpriteRenderer>();

        // Set the initial color of the rectangle
        spriteRenderer1.sprite = Released;
        //currentcolor = "red";
    }

    // Called when another Collider2D enters the trigger area
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the one you're interested in (e.g., player)
        if (other.CompareTag("Human") || other.CompareTag("Boulder"))
        {
            PplOnButton++;
            if(PplOnButton == 1)
            {
                if (FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("Button");
                }
                IsPressed = true;
            }
            //CalculateButton();
            //Toggle(true);
        }
    }

    

    // Called when another Collider2D exits the trigger area
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object exiting the trigger is the one you're interested in (e.g., player)
        if (other.CompareTag("Human") || other.CompareTag("Boulder"))
        {
            //Toggle(false);
            PplOnButton--;
            if(PplOnButton == 0) 
            {
                if (FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("Button");
                }
                IsPressed = false;
            }
            //CalculateButton();

        }
    }

    public void CalculateButton()
    {
        if (PplOnButton <= 0)
        {
                
            IsPressed = false;
                
        }
       
        if(PplOnButton > 0)
        {
                
            IsPressed = false;
        }
    }
}
