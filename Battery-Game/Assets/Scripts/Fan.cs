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
    public MetalPad input;
<<<<<<< HEAD
=======
    public Gate gateInput;
    public SpriteRenderer effect;
>>>>>>> main
    bool on;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
<<<<<<< HEAD
        on = input.active;
=======
        if (input != null) { on = input.active; }
        else { on = gateInput.active; }
        effect.enabled = on;
>>>>>>> main

        if(inFan > 0 && player.GetComponent<Rigidbody2D>().velocity.y < maxSpeed)
        {
            if (on) { player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * strength * Time.deltaTime); }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { inFan ++; }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { inFan --; }
    }
}
