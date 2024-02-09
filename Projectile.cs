using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public LayerMask whatissolid;
    public string TargetTag;
    public float Damage;
    public float Impact;
    public Vector2 CurrentDirection;

    public bool CanDamage = true;
    public bool CanImpact = true;

    //public string SoundName;
    //public float ShakeAmount;
    //public float SpearForce;
    //public AudioMixer secondaudio;


    public GameObject HitEffect;
    GameObject Hit;

    //public GameObject PopUpGo;
    //GameObject InstGo;
    //bool InstantiatedPU = false;

    public bool VisuallyDestroyed = false;
    //public string HitSoundName;

    //AudioManager AM;


    void Awake()
    {
        //Damage = Damage + Random.Range(-3, +4);
        //AM = FindObjectOfType<AudioManager>();
        Invoke("Dest", 8f);

    }

    private void Start()
    {
        CurrentDirection = transform.right;

        if (!VisuallyDestroyed)
        {
            if (this.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
            {
                //this.GetComponent<Rigidbody2D>().AddForce(transform.right * SpearForce, ForceMode2D.Impulse);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!VisuallyDestroyed)
        {
            if (this.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                //this.GetComponent<Rigidbody2D>().velocity = CurrentDirection * speed;
            }

            if (this.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
            {
                Vector2 dir = this.GetComponent<Rigidbody2D>().velocity;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
            }

        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -transform.right, 0.25f, whatissolid);

        if (hitInfo.collider != null)
        {
            //AM.Play(HitSoundName);

            if (hitInfo.rigidbody != null && CanImpact)
            {
                if (hitInfo.rigidbody.bodyType == RigidbodyType2D.Static)
                {

                    CanImpact = false;
                    /*
                    Vector2 BounceDir = Vector2.Reflect(this.GetComponent<Rigidbody2D>().velocity, hitInfo.normal);
                    //CurrentDirection = -BounceDir;

                    float BounceAngle = -Mathf.Atan2(BounceDir.x, BounceDir.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(BounceAngle, transform.forward);
                    */
                }
                else
                {
                    hitInfo.rigidbody.velocity = Vector2.zero;
                    hitInfo.rigidbody.AddForce(hitInfo.normal * Impact, ForceMode2D.Impulse);
                    CanImpact = false;

                }
            }

            
            if (hitInfo.collider.GetComponent<CharacterHealth>() == true)
            {
                if(hitInfo.collider.gameObject.CompareTag(TargetTag) || hitInfo.collider.gameObject.CompareTag("Dummy"))
                {
                    if (CanDamage)
                    {
                        
                        hitInfo.collider.GetComponent<CharacterHealth>().DealDam(Damage);
                        CanDamage = false;
                    }
                }
            }
            
            VisDest(new Vector3(hitInfo.point.x, hitInfo.point.y, transform.position.z), Quaternion.LookRotation(-hitInfo.normal));

        }
    }



    public void VisDest(Vector3 Pos, Quaternion Rot)
    {
        
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.GetChild(0).gameObject.SetActive(false);
        CanDamage = false;
        CanImpact = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
        if (!VisuallyDestroyed)
        {
            //FindObjectOfType<AudioManager>().Play(SoundName);
            Hit = Instantiate(HitEffect, Pos, Rot);
            Hit.transform.SetParent(this.transform);
        }

        //CameraShaker.Instance.ShakeOnce(ShakeAmount, 4f, 0.1f, 1f);

        Invoke("Dest", 3f);
        VisuallyDestroyed = true;
    }

    public void Dest()
    {
        // Destroy(InstGo);
        Destroy(this.gameObject);
    }


    public void SetVolume(float volume)
    {
        //secondaudio.SetFloat("NEW", volume);
    }
}
