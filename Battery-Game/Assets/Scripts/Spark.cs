using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    float sparkVelocity = 5f;
    public List<Vector2> points;
    public int index = 0;
    public MetalBox metalBox;
    public MetalPad metalPad;
    public Gate gate;

    void Update()
    {
        if (Mathf.Round(transform.position.x * 100f) == Mathf.Round(points[index].x * 100f) && (Mathf.Round(transform.position.y * 100f) == Mathf.Round(points[index].y * 100f))) {
            index++;
            if(index >= points.Count) 
            {
                GameObject.Destroy(gameObject);
            }
        }
        if (index < points.Count) { transform.position = Vector2.MoveTowards(transform.position, points[index], sparkVelocity * Time.deltaTime); }
        if(metalBox != null)
        {
            if(!metalBox.active) { GameObject.Destroy(gameObject); }
        }
        if (metalPad != null)
        {
            if (!metalPad.active) { GameObject.Destroy(gameObject); }
        }
        if (gate != null)
        {
            if (!gate.active) { GameObject.Destroy(gameObject); }
        }
    }

}
