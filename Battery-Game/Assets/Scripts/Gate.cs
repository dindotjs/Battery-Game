using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool active;
    public MetalPad input;
    public int type;
    public float delayTime = 1f;
    bool delayGateCounting = false;

    void Update()
    {
        if (type == 0) 
        { 
            active = !input.active; 
        }
        else if(type == 1)
        {
            if(input.active && !active && !delayGateCounting)
            {
                StartCoroutine(DelayGate());
            }
            else if(!input.active && active && !delayGateCounting)
            {
                StartCoroutine(DelayGate());
            }
        }
    }

    IEnumerator DelayGate()
    {
        delayGateCounting = true;
        yield return new WaitForSeconds(delayTime);
        active = input.active;
        delayGateCounting = false;
    }
}
