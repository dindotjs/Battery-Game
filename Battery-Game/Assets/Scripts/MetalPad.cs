using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPad : MonoBehaviour
{
    public bool active = false;
    LineRenderer wire;
    public GameObject attachedObject;
    Color32 wireColorOff = new Color32(0x23, 0x1B, 0x23, 0xFF);
    Color32 wireColorOn = new Color32(0xF9, 0xC2, 0x2B, 0xFF);

    bool sentSpark = false;
    public GameObject sparkPrefab;

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
            wire.startColor = wireColorOff;
            wire.endColor= wireColorOff;
        }

        if (attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }

        if (active && !sentSpark)
        {
            StartCoroutine(SendSpark());
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

    IEnumerator SendSpark()
    {
        sentSpark = true;
        Spark spark = GameObject.Instantiate(sparkPrefab, transform.position, Quaternion.identity).GetComponent<Spark>();
        spark.metalPad = this;
        for (int i = 0; i < wire.positionCount; i++)
        {
            spark.points.Add((Vector2)wire.GetPosition(i));
        }
        yield return new WaitForSeconds(1f);
        sentSpark = false;
    }
}
