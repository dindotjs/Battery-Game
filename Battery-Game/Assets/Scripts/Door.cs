using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Door : MonoBehaviour
{
    ElectricInput input;
    public GameObject door;
    Vector2 doorDir = new Vector2(0f, -2f); //go down by this - ie. door.transform.position + doorDir;

    private void Start()
    {
        input = GetComponent<ElectricInput>();
    }

    void Update()
    {
        if(input.on)
        {
            door.GetComponent<SpriteRenderer>().color = Color.grey;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            door.GetComponent<SpriteRenderer>().color = Color.white;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
