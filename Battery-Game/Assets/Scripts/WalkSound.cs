using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public float soundToPlay = -1.0f; //this with designate which sound to play. -1 means don’t play any sound. This is a float because then the animation window can access it.
    public AudioSource walk; //this holds the sounds
    bool playedSound = false;

    void Update()
    {
        //put the following in update
        if (soundToPlay > -1.0f)
        {//if the sound is greater than the value for not playing a sound
            if(!playedSound)
            {
                SoundManager.PlaySoundRandom(walk, 0.95f, 1.05f);//play the sound, casting the float to an int so that the audio source can use it
                //soundToPlay = -1.0f;//set it back to zero to keep this from looping back around and playing the sound again.
                playedSound = true;
            }
        }
        else
        {
            playedSound = false;
        }
    }
}
