using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{

    public GameObject player;
    public GameObject battery;
    List<Vector2> points = new List<Vector2>();
    LineRenderer line;
    int hittable;
    float outLength = 0.01f;

    void Start()
    {
        points.Add(battery.transform.position);
        points.Add(player.transform.position);
        line = GetComponent<LineRenderer>();
        hittable = LayerMask.GetMask("Ground");
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        AddTension();
        if (points.Count > 2) { RemoveTension(); }
        UpdatePoints();
    }

    void UpdatePoints()
    {
        points[0] = battery.transform.position;
        points[points.Count - 1] = player.transform.position;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    void AddTension()
    {
        //todo - add/remove tension from battery to closest point

        Vector2 direction = (points[points.Count - 2] - points[points.Count - 1]).normalized;
        float distance = (points[points.Count - 2] - points[points.Count - 1]).magnitude;
        RaycastHit2D hit = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);
        if(hit.collider != null)
        {
            if(Mathf.Round(hit.point.x * 100) == Mathf.Round(points[points.Count-2].x * 100) && Mathf.Round(hit.point.y * 100) == Mathf.Round(points[points.Count - 2].y * 100)) { return;  }
            Vector2 newPoint = hit.point + hit.normal * outLength;
            points[points.Count - 1] = newPoint;
            points.Add(player.transform.position);
            line.positionCount++;
        }
    }

    void RemoveTension()
    {
        Vector2 direction = (points[points.Count - 3] - points[points.Count - 1]).normalized;
        float distance = (points[points.Count - 3] - points[points.Count - 1]).magnitude;
        RaycastHit2D hit = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);

        if(hit.collider == null)
        {
            Debug.Log("Removing Point");
            direction = (points[points.Count - 2] - points[points.Count - 1]).normalized;
            distance = (points[points.Count - 2] - points[points.Count - 1]).magnitude;
            RaycastHit2D hit2 = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);
            if(hit2.collider != null) { return; }

            points[points.Count - 2] = points[points.Count - 1];
            points.RemoveAt(points.Count-1);
            line.positionCount--;
        }
    }
}
