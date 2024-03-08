using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public bool active;
    public MetalPad metalPad;
    public MetalBox metalBox;
    public Gate gateInput;

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

    void Update()
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


        if (metalPad != null)
        {
            active = metalPad.active;
        }
        else if(metalBox != null)
        {
            active = metalBox.active;
        }
        else if(gateInput != null)
        {
            active = gateInput.active;
        }


        if (active && !sentSpark)
        {
            StartCoroutine(SendSpark());
        }
    }

    IEnumerator SendSpark()
    {
        sentSpark = true;
        Spark spark = GameObject.Instantiate(sparkPrefab, transform.position, Quaternion.identity).GetComponent<Spark>();
        spark.wire = this;
        for (int i = 0; i < wire.positionCount; i++)
        {
            spark.points.Add((Vector2)wire.GetPosition(i));
        }
        if (attachedObject != null) { spark.attachedObject = attachedObject; }
        yield return new WaitForSeconds(1f);
        sentSpark = false;
    }
}
