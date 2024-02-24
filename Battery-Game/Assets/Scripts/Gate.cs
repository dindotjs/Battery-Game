using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    bool on;
    bool on2;
    public bool active;
    public MetalPad padInput;
    public MetalBox boxInput;
    public Gate gateInput;
    public MetalPad padInput2;
    public MetalBox boxInput2;
    public Gate gateInput2;
    // 0 = not | 1 = delay | 2 = and 
    public int type;
    public float delayTime = 1f;
    bool delayGateCounting = false;

    LineRenderer wire;
    public GameObject attachedObject;

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
    }

    void Update()
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

        if (padInput != null)
        {
            on = padInput.active;
        }
        else if(boxInput != null)
        {
            on = boxInput.active;
        }
        else if(gateInput != null)
        {
            on = gateInput.active;
        }

        if(padInput2 != null)
        {
            on2 = padInput2.active;
        }
        else if(boxInput2 != null)
        {
            on2 = boxInput2.active;
        }
        else if(gateInput2 != null)
        {
            on2 = gateInput2.active;
        }

        if(attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }


        if (type == 0) 
        {
            active = !on; 
        }
        else if(type == 1)
        {
            if(on && !active && !delayGateCounting)
            {
                StartCoroutine(DelayGate());
            }
            else if(!on && active && !delayGateCounting)
            {
                StartCoroutine(DelayGate());
            }
        }
        else if(type == 2)
        {
            if(on && on2)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }
    }

    IEnumerator DelayGate()
    {
        delayGateCounting = true;
        yield return new WaitForSeconds(delayTime);
        active = on;
        delayGateCounting = false;
    }
}
