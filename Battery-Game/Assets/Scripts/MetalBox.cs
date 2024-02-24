using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBox : MonoBehaviour
{
    public bool active;
    bool batteryOn;
    LineRenderer wire;
    public GameObject attachedObject;

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (active)
        {
            wire.startColor = Color.yellow;
            wire.endColor = Color.yellow;
        }
        else
        {
            wire.startColor = Color.black;
            wire.endColor = Color.black;
        }

        if (batteryOn)
        {
            active = true;
        }

        if (attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            batteryOn = true;
            active = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            batteryOn = false;
            active = false;
        }
    }
}
