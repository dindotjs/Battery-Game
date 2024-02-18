using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cable : MonoBehaviour
{
    //todo - when removing a point, loop through 10-20 points on that line, and see if you can see them all. if you cant see one, then dont remove
    public GameObject player;
    public GameObject battery;
    public List<Vector2> points = new List<Vector2>();
    public List<Vector2> midPoints = new List<Vector2>();
    List<float> lengths = new List<float>();
    public List<GameObject> attachedObject = new List<GameObject>();
    public float totalLength;
    LineRenderer line;
    int hittable;
    float outLength = 0.01f;
    float midPointFactor = 11 / 12;

    float forceStrength = 3000f;
    float maxLength = 20f;

    void Start()
    {
        points.Add(battery.transform.position);
        points.Add(player.transform.position);
        attachedObject.Add(battery);
        attachedObject.Add(player);
        midPoints.Add((points[1] + points[0]) * midPointFactor);
        lengths.Add((points[1] - points[0]).magnitude);
        line = GetComponent<LineRenderer>();
        hittable = LayerMask.GetMask("Ground");
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        AddTension();
        if(points.Count > 2) { RemoveTension(); }
        
        UpdatePoints();
        UpdateMidpoints();
        lengths[lengths.Count - 1] = (points[points.Count - 1] - points[points.Count - 2]).magnitude;
        if(lengths.Count > 2)
        {
            lengths[lengths.Count-2] = (points[points.Count-2] - points[points.Count-3]).magnitude;
        }
        CheckMoving();
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
    void UpdateMidpoints()
    {
        for(int i = 0; i < midPoints.Count; i++)
        {
            midPoints[i] = (points[i + 1] + points[i]) * midPointFactor;
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
            attachedObject[attachedObject.Count - 1] = hit.collider.gameObject;
            points.Add(player.transform.position);
            attachedObject.Add(player);
            line.positionCount++;
            lengths.Add(0f);
            midPoints.Add(newPoint);

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
            direction = (midPoints[midPoints.Count-2] - points[points.Count - 1]).normalized;
            distance = (midPoints[midPoints.Count -2] - points[points.Count - 1]).magnitude;
            RaycastHit2D hit2 = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);
            Debug.DrawLine(points[points.Count - 1], midPoints[midPoints.Count - 1]);
            if(hit2.collider != null) { return; }

            points[points.Count - 2] = points[points.Count - 1];
            attachedObject[attachedObject.Count - 2] = attachedObject[attachedObject.Count - 1];
            points.RemoveAt(points.Count-1);
            attachedObject.RemoveAt(attachedObject.Count-1);
            line.positionCount--;
            lengths.RemoveAt(lengths.Count-1);
            midPoints.RemoveAt(midPoints.Count - 1);

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
        if(totalLength > maxLength)
        {
            Vector2 direction = points[points.Count-2] - points[points.Count - 1];
            float extension = totalLength - maxLength;
            player.GetComponent<Rigidbody2D>().AddForce(direction.normalized * Mathf.Sqrt(extension) * forceStrength * Time.deltaTime);
        }
    }
    void CheckMoving()
    {
        for(int i = 0; i < attachedObject.Count; i++)
        {
            if (attachedObject[i].GetComponent<MovingPlatform>() != null)
            {
                points[i] += attachedObject[i].GetComponent<MovingPlatform>().difference;
                lengths[i - 1] = (points[i] - points[i - 1]).magnitude;
                lengths[i] = (points[i + 1] - points[i]).magnitude;
            }
        }
    }
}
