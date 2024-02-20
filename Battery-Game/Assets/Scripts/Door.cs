using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Door : MonoBehaviour
{
    ElectricInput input;

    private void Start()
    {
        input = GetComponent<ElectricInput>();
    }

    void Update()
    {
        if(input.on)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
