using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{

    public AudioSource hitSound;
    public bool justThrown = false;

    private void Update()
    {
        if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 4f) { justThrown = true; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (justThrown)
        {
            justThrown = false;
            SoundManager.PlaySoundRandom(hitSound, 0.95f, 1f);
        }
    }
    
}
