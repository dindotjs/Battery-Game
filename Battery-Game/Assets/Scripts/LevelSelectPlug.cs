using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPlug : MonoBehaviour
{
    LevelManager levelManager;
    public int level;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<SpriteRenderer>().enabled = false;
            other.GetComponent<PlayerMovement>().plugin.enabled = true;
            other.GetComponent<PlayerMovement>().pluggedIn = true;
            other.transform.position = transform.position;
            other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            GetComponent<AudioSource>().Play();
            StartCoroutine(levelManager.LoadScene(level, 0.8f));
        }
    }
}
