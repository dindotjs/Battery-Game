using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public float soundToPlay = -1.0f; 
    public AudioSource walk; 
    public bool playedSound = false;

    void Update()
    {
        if (soundToPlay > -1.0f)
        {
            if(!playedSound)
            {
                SoundManager.PlaySoundRandom(walk, 0.95f, 1.05f);
                playedSound = true;
            }
        }
        else
        {
            playedSound = false;
        }
    }
}
