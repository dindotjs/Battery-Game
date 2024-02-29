using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBox : MonoBehaviour
{
    public bool active;
    bool batteryOn;
    LineRenderer wire;
    public GameObject attachedObject;
    Color32 wireColorOff = new Color32(0x23, 0x1B, 0x23, 0xFF);
    Color32 wireColorOn = new Color32(0xF9, 0xC2, 0x2B, 0xFF);
    public GameObject sparkPrefab;
    bool sentSpark = false;

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (active)
        {
            wire.startColor = wireColorOn;
            wire.endColor = wireColorOn;
        }
        else
        {
            wire.startColor = wireColorOff;
            wire.endColor = wireColorOff;
        }

        if (batteryOn)
        {
            active = true;
        }

        if (attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }

        if(active && !sentSpark)
        {
            StartCoroutine(SendSpark());
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

    IEnumerator SendSpark()
    {
        sentSpark = true; 
        Spark spark = GameObject.Instantiate(sparkPrefab, transform.position, Quaternion.identity).GetComponent<Spark>();
        spark.metalBox = this;
        for(int i = 0; i < wire.positionCount; i++)
        {
            spark.points.Add((Vector2)wire.GetPosition(i));
        }
        yield return new WaitForSeconds(1f);
        sentSpark = false;
    }
}
