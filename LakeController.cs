//using UnityEditor.PackageManager;
using UnityEngine;

public class RectangleController : MonoBehaviour
{
    private GameObject playerCircle;
    public float detectionRange = 5f;

    public KeyCode interactKey = KeyCode.E;

    private SpriteRenderer spriteRenderer;

    public bool isInRange = false;
    public bool IsAcid = true;

    public PlayerController pc;

    public Sprite Acid;
    public Sprite Water;

    public ParticleSystem AcidDefault;
    public ParticleSystem ChangeToAcid;
    public ParticleSystem ChangeToWater;

    public GameObject AcidCollider;
    public GameObject WaterCollider;



    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Acid; // Initial color of the rectangle

        if (IsAcid)
        {
            FindObjectOfType<AudioManager>().Play("Bubble");
            AcidDefault.Play();
            ChangeToAcid.Play();
            spriteRenderer.sprite = Acid; // Change color to green
            IsAcid = true;
            WaterCollider.gameObject.SetActive(false);
            AcidCollider.gameObject.SetActive(true);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Bubble");
            AcidDefault.Stop();
            ChangeToWater.Play();
            spriteRenderer.sprite = Water; // Change color back to blue
            IsAcid = false;
            WaterCollider.gameObject.SetActive(true);
            AcidCollider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey) && pc.ActiveHuman.gameObject.GetComponent<scientist>() != null)
        {
            ChangeColor();
        }
        if (pc.GhostMode)
        {
            playerCircle = pc.Ghost;
        }
        else if (!pc.GhostMode)
        {
            playerCircle = pc.ActiveHuman;
        }
        
        /*
        if(transform.GetChild(0).GetComponent<LakeKiller>().HumanIsInside)
        {
            this.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
        }
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerCircle)
        {
            if (other.GetComponent<scientist>() != null) // Check if the colliding object has a Scientist script
            {
                isInRange = true;
                this.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().HighlightedMat;
            }
            /*
            if (spriteRenderer.sprite == Acid && (other.CompareTag("Human"))||other.CompareTag("Ghost")) // Check if the colliding object is a player with the "Player" tag
            {
                //FindObjectOfType<LevelManager>().LoseThisLevel("Lmao Skill issue");
                Debug.Log("Game Over"); // Display game over message in console
            }
            */
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerCircle)
        {
            if (other.GetComponent<scientist>() != null) // Check if the colliding object has a Scientist script
            {
                isInRange = false;
                this.gameObject.GetComponent<Renderer>().material = FindObjectOfType<MySceneManager>().DefaultMat;
            }
        }
    }

    void ChangeColor()
    {
        FindObjectOfType<AudioManager>().Play("Bubble");
        if (IsAcid)
        {
            // Go to Water
            AcidDefault.Stop();
            ChangeToWater.Play();
            spriteRenderer.sprite = Water; // Change color back to blue
            IsAcid = false;
            WaterCollider.gameObject.SetActive(true);
            AcidCollider.gameObject.SetActive(false);
        }
        else
        {
            // Go To Acid
            AcidDefault.Play();
            ChangeToAcid.Play();
            spriteRenderer.sprite = Acid; // Change color to green
            IsAcid = true;
            WaterCollider.gameObject.SetActive(false);
            AcidCollider.gameObject.SetActive(true);
        }
    }
}
