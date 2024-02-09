using UnityEngine;

public class multiplebuttongate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public bool IsOpen = false;
    public Sprite Open;
    public Sprite Closed;

    public ParticleSystem OpenParticles;

    private GameObject[] buttons;

    public KeyCode interactKey = KeyCode.E;

    public bool GhostInRadius = false;

    GameObject Ghost;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.color = Color.blue;
        Ghost = FindObjectOfType<PlayerController>().gameObject;
        
    }

    void Update()
    {
        bool allButtonsGreen = CheckAllButtonsGreen();

        if (allButtonsGreen)
        {   
            if(!IsOpen)
            {
                FindObjectOfType<AudioManager>().Play("DoorOpen");
                spriteRenderer.sprite = Open;
                OpenParticles.Play();
                IsOpen = true;
            }
            
        }
        else
        {
            if (IsOpen)
            {
                FindObjectOfType<AudioManager>().Play("DoorClose");
            }
            spriteRenderer.sprite = Closed; // Change to desired default color
            IsOpen = false;
        }

        if(GhostInRadius)
        {
            if (Ghost.gameObject.GetComponent<PlayerController>().GhostMode && IsOpen)
            {
                FindObjectOfType<LevelManager>().WinThisLevel();
                Debug.Log("Level Complete"); // Display game over message in console
                
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            //print("GHOSTT");
            if (other.gameObject.GetComponent<PlayerController>().GhostMode && IsOpen)
            {
                GhostInRadius = true;
                this.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().HighlightedMat;
            }
                
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            if (other.gameObject.GetComponent<PlayerController>().GhostMode && IsOpen)
            {
                //print("GHOSTT");
                GhostInRadius = false;
                if(FindObjectOfType<MySceneManager>() != null)
                {
                    this.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
                }
                
            }
        }
    }


    bool CheckAllButtonsGreen()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");

        foreach (GameObject button in buttons)
        {
            if(button.GetComponent<ColorChangeOnTrigger>().IsPressed != true)
            {
                return false;
            }
        }

        return true;
    }
}
