using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Door : MonoBehaviour
{
    ElectricInput input;
    public GameObject door;
    Vector2 end = Vector2.down * 2;
    float doorSpeed = 5f;

    private void Start()
    {
        input = GetComponent<ElectricInput>();
    }

    void Update()
    {
        if(input.on)
        {
            if(door.GetComponent<BoxCollider2D>().enabled && Time.timeSinceLevelLoad > 0.05f) { SoundManager.Play(gameObject); }
            door.transform.localPosition = Vector2.Lerp(door.transform.localPosition, end, doorSpeed * Time.deltaTime);
            //door.GetComponent<SpriteRenderer>().color = Color.grey;
            door.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            if (!door.GetComponent<BoxCollider2D>().enabled && Time.timeSinceLevelLoad > 0.05f) { SoundManager.PlayRandom(gameObject, 1.05f, 1.1f); }
            door.transform.localPosition = Vector2.Lerp(door.transform.localPosition, Vector2.zero, doorSpeed * Time.deltaTime);
            //door.GetComponent<SpriteRenderer>().color = Color.white;
            door.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
