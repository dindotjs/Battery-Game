using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    float sparkVelocity = 5f;
    public List<Vector2> points;
    public int index = 0;

    void Update()
    {
        if (Mathf.Round(transform.position.x * 10f) == Mathf.Round(points[index].x * 10f) && (Mathf.Round(transform.position.y * 10f) == Mathf.Round(points[index].y * 10f))) {
            index++;
            if(index >= points.Count) 
            {
                GameObject.Destroy(gameObject);
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[0], sparkVelocity * Time.deltaTime);
    }

}
