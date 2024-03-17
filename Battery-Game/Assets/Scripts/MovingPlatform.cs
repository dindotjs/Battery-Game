using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 difference;
    public Vector2 startPos;
    public Vector2 endPos;
    ElectricInput input;
    bool on;
    float moveSpeed = 1f;
    Vector2 lastPos;
    bool playerOn;
    bool batteryOn;
    GameObject player;
    GameObject battery;

    bool moving = false;
    bool movingBack = false;
    Animator anim;

    public List<Animator> beams = new List<Animator>();
    float tolerence = 2500f;

    public AudioSource move;
    public AudioSource stop;
    bool madeNoise = false;


    private void Start()
    {
        startPos = transform.position;
        input = GetComponent<ElectricInput>();
        anim = GetComponent<Animator>();
        difference = Vector2.zero;
    }
    void Update()
    {
        on = input.on; 

        if(on)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, Time.deltaTime * moveSpeed);
            /*if(Mathf.Round(transform.position.x * 100) == Mathf.Round(endPos.x * 100) && Mathf.Round(transform.position.y * 100) == Mathf.Round(endPos.y * 100))
            {
                transform.position = endPos;
            }*/
            difference = (Vector2)transform.position - lastPos;
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, startPos, Time.deltaTime * moveSpeed);
            difference = (Vector2)transform.position - lastPos;
        }

        if((Mathf.Round(transform.position.x * Time.deltaTime * tolerence) == Mathf.Round(endPos.x * Time.deltaTime * tolerence) && Mathf.Round(transform.position.y * Time.deltaTime * tolerence) == Mathf.Round(endPos.y * Time.deltaTime * tolerence)) && on)
        {
            if (on && moving) { SoundManager.PlaySound(stop); }
            transform.position = endPos;
            moving = false;
            difference = (Vector2)transform.position - lastPos;
            madeNoise = false;
        }
        else
        {
            moving = true;
        }

        if((Mathf.Round(transform.position.x * Time.deltaTime * tolerence) == Mathf.Round(startPos.x * Time.deltaTime * tolerence) && Mathf.Round(transform.position.y * Time.deltaTime * tolerence) == Mathf.Round(startPos.y * Time.deltaTime * tolerence)) && !on)
        {
            if (!on && movingBack) { SoundManager.PlaySound(stop); }
            transform.position = startPos;
            movingBack = false;
            difference = (Vector2)transform.position - lastPos;
            madeNoise = false;
        }
        else
        {
            movingBack = true;
        }

        lastPos = transform.position;


        if(playerOn && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            player.transform.position += (Vector3)difference;

        }

        if (batteryOn)
        {
            battery.transform.position += (Vector3)difference;

        }

        if((on && moving) || (!on && movingBack)) 
        {
            if(!madeNoise)
            {
                StartCoroutine(MakeNoise());
            }
        }

        AnimatePlatform();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerOn = true;
            player = collision.gameObject;
        }
        else if(collision.gameObject.tag == "Battery")
        {
            batteryOn = true;
            battery = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOn = false;
        }
        else if (collision.gameObject.tag == "Battery")
        {
            batteryOn = false;
        }
    }

    IEnumerator MakeNoise()
    {
        madeNoise = true;
        yield return new WaitForSeconds((0.3f / (difference.magnitude/Time.deltaTime)));
        if ((on && moving) || (!on && movingBack)) { SoundManager.PlaySoundRandom(move, 0.9f, 1.1f); }
        madeNoise = false;
    }

    void AnimatePlatform()
    {
        if (on && moving)
        {
            if (difference.y > 0)
            {
                anim.Play("PlatformMoving");
            }
            else
            {
                anim.Play("PlatformDown");
            }
        }   
        else if(!on && movingBack)
        {
            if (difference.y > 0)
            {
                anim.Play("PlatformMoving");
            }
            else
            {
                anim.Play("PlatformDown"); // change to up/down
            }
        }
        else if (on)
        {
            anim.Play("PlatformOn");
        }
        else
        {
            anim.Play("PlatformStill");
        }

        if((on && moving) || (!on && movingBack) )
        {
            for (int i = 0; i < beams.Count; i++)
            {
                beams[i].speed = 2f;
                if(difference.y > 0)
                {
                    beams[i].Play("BeamDown");
                }
                else
                {
                    beams[i].Play("BeamUp");
                }
            }
        }
        else
        {
            for (int i = 0; i < beams.Count; i++)
            {
                beams[i].Play("StillBeam");
            }
        }
    }
}
