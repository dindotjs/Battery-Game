using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBody : MonoBehaviour
{
    public Vector2 difference;
    Vector2 lastPos;

    void Update()
    {
        difference = (Vector2)transform.position - lastPos;
        lastPos = transform.position;
    }
}
