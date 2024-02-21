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
    public MetalPad padInput2;
    public MetalBox boxInput2;
    // 0 = not | 1 = delay | 2 = and 
    public int type;
    public float delayTime = 1f;
    bool delayGateCounting = false;

    void Update()
    {
        if(padInput != null)
        {
            on = padInput.active;
        }
        else if(boxInput != null)
        {
            on = boxInput.active;
        }

        if(padInput2 != null)
        {
            on2 = padInput2.active;
        }
        else if(boxInput2 != null)
        {
            on2 = boxInput2.active;
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
