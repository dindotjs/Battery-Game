using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    bool paused = false;
    AudioSource music;
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("MusicManager").Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        music = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(!paused)
            {
                paused = true;
                music.Pause();
            }
            else
            {
                music.Play();
                paused = false;
            }
        }
    }
}
