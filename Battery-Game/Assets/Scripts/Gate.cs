using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool active;
    public MetalPad input;
    public int type;
    void Update()
    {
        if (type == 0) { active = !input.active; }
    }
}
