using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Door : MonoBehaviour
{
    ElectricInput input;
    public GameObject door;
    Vector2 end = Vector2.down * 2;
    Vector2 orgin;
    float doorSpeed = 5f;

    private void Start()
    {
        input = GetComponent<ElectricInput>();
        orgin = door.transform.position;
    }

    void Update()
    {
        if(input.on)
        {
            door.transform.position = Vector2.Lerp(door.transform.position, orgin + end, doorSpeed * Time.deltaTime);
            //door.GetComponent<SpriteRenderer>().color = Color.grey;
            door.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            door.transform.position = Vector2.Lerp(door.transform.position, orgin, doorSpeed * Time.deltaTime);
            //door.GetComponent<SpriteRenderer>().color = Color.white;
            door.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
