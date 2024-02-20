using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 difference;
    public Vector2 startPos;
    public Vector2 endPos;
    ElectricInput input;
    bool on;
    float moveSpeed = 1f;
    Vector2 lastPos;
    private void Start()
    {
        startPos = transform.position;
        input = GetComponent<ElectricInput>();
    }
    void Update()
    {
        on = input.on; 
        
        if(on)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, Time.deltaTime * moveSpeed);
            if(Mathf.Round(transform.position.x * 100) == Mathf.Round(endPos.x * 100) && Mathf.Round(transform.position.y * 100) == Mathf.Round(endPos.y * 100))
            {
                transform.position = endPos;
            }
            difference = (Vector2)transform.position - lastPos;
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, startPos, Time.deltaTime * moveSpeed);
            difference = (Vector2)transform.position - lastPos;
        }
        lastPos = transform.position;
    }
}
