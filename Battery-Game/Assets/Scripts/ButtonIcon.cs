using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIcon : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>(2);
    float delay = 0.5f;
    bool changingSprite = false;
    int currentSprite;
    Transform player;
    float playerDistance = 5f;
    SpriteRenderer sr;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if((player.position - transform.position).magnitude < playerDistance)
        {
            sr.color = new Color(1f, 1f, 1f, Mathf.Clamp(playerDistance - (player.position - transform.position).magnitude, 0f, 1f));
        }
        else
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
        }
        //GetComponent<SpriteRenderer>().enabled = (player.position - transform.position).magnitude < playerDistance;

        if(!changingSprite)
        {
            StartCoroutine(NextSprite());
        }
    }

    IEnumerator NextSprite()
    {
        changingSprite = true;
        yield return new WaitForSeconds(delay);
        currentSprite++;
        if(currentSprite > 1) { currentSprite = 0; }
        GetComponent<SpriteRenderer>().sprite = sprites[currentSprite];
        changingSprite = false;
    }
}
