using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPad : MonoBehaviour
{
    public bool active = false;
    LineRenderer wire;
    public GameObject attachedObject;
    Color32 wireColorOn = new Color32(0xF9, 0xC2, 0x2B, 0xFF);

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(active)
        {
            wire.startColor = wireColorOn;
            wire.endColor = wireColorOn;
        }
        else
        {
            wire.startColor = Color.black;
            wire.endColor= Color.black;
        }

        if (attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            active = true;
            collision.gameObject.transform.position = transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
