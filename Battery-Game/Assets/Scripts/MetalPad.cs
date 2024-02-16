using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPad : MonoBehaviour
{
    public bool active = false;
    LineRenderer wire;

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(active)
        {
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
