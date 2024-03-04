using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Fan : MonoBehaviour
{
    float strength = 4000f;
    float maxSpeed = 10f;
   public int inFan = 0;
    GameObject player;
    public ParticleSystem effect;
    public SpriteMask mask;
    ElectricInput input;
    bool on;
    Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        input = GetComponent<ElectricInput>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        on = input.on;
        mask.enabled = on;
        //effect.enabled = on;
        /*if(on)
        {
            effect.Play();
        }
        else
        {
            effect.Stop();
        }*/
        if(inFan > 0 && player.GetComponent<Rigidbody2D>().velocity.y < maxSpeed)
        {
            if(on && player.GetComponent<Rigidbody2D>().velocity.y < 0 && player.transform.position.y - transform.position.y < 2) { player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0f); }
            if (on) { player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strength * Time.deltaTime); }
        }
        Animate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { inFan ++; }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { inFan --; }
    }

    private void Animate()
    {
        if(on)
        {
            anim.Play("FanSpin");
        }
        else
        {
            anim.Play("FanIdle");
        }
    }
}
