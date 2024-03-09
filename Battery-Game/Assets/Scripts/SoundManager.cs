using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static void Play(GameObject orgin)
    {
        orgin.GetComponent<AudioSource>().pitch = 1f;
        orgin.GetComponent<AudioSource>().Play();
    }
    public static void PlayRandom(GameObject orgin, float min, float max)
    {
        float pitch = Random.Range(min, max);
        orgin.GetComponent<AudioSource>().pitch = pitch;
        orgin.GetComponent<AudioSource>().Play();
    }

    public static void PlaySound(AudioSource audio)
    {
        audio.pitch = 1f;
        audio.Play();
    }

    public static void PlaySoundRandom(AudioSource audio, float min, float max)
    {
        float pitch = Random.Range(min, max);
        audio.pitch = pitch;
        audio.Play();
    }

    public static void PlaySoundBackwards(AudioSource audio)
    {
        audio.pitch = -1f;
        audio.timeSamples = audio.clip.samples - 1;
        audio.Play();
    }
}
