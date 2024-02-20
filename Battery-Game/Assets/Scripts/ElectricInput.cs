using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricInput : MonoBehaviour
{
    public MetalPad metalPad;
    public MetalBox metalBox;
    public Gate gate;
    public bool on;

    void Update()
    {
        if (metalPad != null)
        {
            on = metalPad.active;
        }
        else if (metalBox != null)
        {
            on = metalBox.active;
        }
        else if (gate != null) 
        {
            on = gate.active;
        }
    }
}
