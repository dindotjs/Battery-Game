using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBox : MonoBehaviour
{
    public bool active;
    bool batteryOn;

    private void Update()
    {
        if(batteryOn)
        {
            active = true;
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
