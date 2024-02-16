using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Door : MonoBehaviour
{
    public MetalPad input;

    void Update()
    {
        if(input.active)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
