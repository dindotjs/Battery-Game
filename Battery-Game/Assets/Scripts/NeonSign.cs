using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSign : MonoBehaviour
{
    ElectricInput input;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<ElectricInput>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(input.on)
        {
            anim.Play("on");
        }
        else
        {
            anim.Play("off");
        }
    }
}
