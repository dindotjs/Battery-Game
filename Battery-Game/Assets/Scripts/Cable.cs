using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Cable : MonoBehaviour
{

    public GameObject player;
    public GameObject battery;
    List<Vector2> points = new List<Vector2>();
    List<float> lengths = new List<float>();
    public float totalLength;
    float baseLength;
    LineRenderer line;
    int hittable;
    float outLength = 0.01f;

    float forceStrength = 3000f;
    float maxLength = 20f;

    void Start()
    {
        points.Add(battery.transform.position);
        points.Add(player.transform.position);
        lengths.Add((points[1] - points[0]).magnitude);
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
        lengths[lengths.Count - 1] = (points[points.Count - 1] - points[points.Count - 2]).magnitude;
        if(lengths.Count > 2)
        {
            lengths[lengths.Count-2] = (points[points.Count-2] - points[points.Count-3]).magnitude;
        }
        totalLength = baseLength + lengths[lengths.Count - 1];
        PullPlayer();
        Debug.Log(totalLength);
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
            lengths.Add(0f);

            /*baseLength = 0f;
            for(int i = 0; i < lengths.Count-1; i++)
            {
                baseLength += lengths[i];
            }*/
        }
    }

    void RemoveTension()
    {
        Vector2 direction = (points[points.Count - 3] - points[points.Count - 1]).normalized;
        float distance = (points[points.Count - 3] - points[points.Count - 1]).magnitude;
        RaycastHit2D hit = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);

        if(hit.collider == null)
        {
            direction = (points[points.Count - 2] - points[points.Count - 1]).normalized;
            distance = (points[points.Count - 2] - points[points.Count - 1]).magnitude;
            RaycastHit2D hit2 = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);
            if(hit2.collider != null) { return; }

            points[points.Count - 2] = points[points.Count - 1];
            points.RemoveAt(points.Count-1);
            line.positionCount--;
            lengths.RemoveAt(lengths.Count-1);

            /*baseLength = 0f;
            for (int i = 0; i < lengths.Count - 1; i++)
            {
                baseLength += lengths[i];
            }*/
        }
    }
    void PullPlayer()
    {
        totalLength = 0f;
        for(int i = 0; i < lengths.Count; i++)
        {
            totalLength += lengths[i];
        }
        Debug.Log(totalLength);
        if(totalLength > maxLength)
        {
            Vector2 direction = points[points.Count-2] - points[points.Count - 1];
            float extension = totalLength - maxLength;
            player.GetComponent<Rigidbody2D>().AddForce(direction.normalized * Mathf.Sqrt(extension) * forceStrength);
        }
    }
}
