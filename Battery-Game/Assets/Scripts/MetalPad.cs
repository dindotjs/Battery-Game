using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPad : MonoBehaviour
{
    public bool active = false;
    LineRenderer wire;

<<<<<<< HEAD
    private void Start()
    {
        wire = GetComponent<LineRenderer>();
    }

=======
>>>>>>> main
    private void Update()
    {
        if(active)
        {
<<<<<<< HEAD
            Debug.Log("On");
=======
>>>>>>> main
            wire.startColor = Color.yellow;
            wire.endColor = Color.yellow;
        }
        else
        {
            wire.startColor = Color.black;
            wire.endColor= Color.black;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            active = false;
        }
    }
}
