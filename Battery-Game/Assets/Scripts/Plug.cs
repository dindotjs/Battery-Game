using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    LevelManager levelManager;
    public GameObject fadePrefab;

    void Start()
    {
        Instantiate(fadePrefab);
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<SpriteRenderer>().enabled = false;
            other.GetComponent<PlayerMovement>().plugin.enabled = true;
            other.GetComponent<PlayerMovement>().pluggedIn = true;
            other.transform.position = transform.position;
            other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            GetComponent<AudioSource>().Play();
            StartCoroutine(levelManager.LoadScene(levelManager.currentScene + 1, 0.8f));
        }
    }
}
