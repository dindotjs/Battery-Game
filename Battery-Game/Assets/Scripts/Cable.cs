using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;

public class Cable : MonoBehaviour
{
    //todo - when removing a point, loop through 10-20 points on that line, and see if you can see them all. if you cant see one, then dont remove
    public GameObject player;
    public GameObject battery;
    public List<Vector2> points = new List<Vector2>();
    public List<Vector2> visualPoints = new List<Vector2>();
    List<float> lengths = new List<float>();
    public List<GameObject> attachedObject = new List<GameObject>();
    public float totalLength;
    LineRenderer line;
    int hittable;
    float outLength = 0.075f;
    float pointTolerence = 0.1f;
    public float midPointFactor = 0.8f;
    float numChecks = 10f;
    float tolerence = 15f;

    float forceStrength = 3000f;
    float maxLength = 20f;

    public SpriteRenderer bar;
    public List<Sprite> barSprites = new List<Sprite>();

    void Start()
    {
        points.Add(battery.transform.position);
        points.Add(player.transform.position);
        visualPoints.Add(battery.transform.position);
        visualPoints.Add(player.transform.position);
        attachedObject.Add(battery);
        attachedObject.Add(player);
        lengths.Add((points[1] - points[0]).magnitude);
        line = GetComponent<LineRenderer>();
        hittable = LayerMask.GetMask("Ground");
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerMovement>().holdingBattery)
        {
            ResetPoints();
        }


        AddTension();
        if(points.Count > 2) { RemoveTension(); }
        
        UpdatePoints();
        lengths[lengths.Count - 1] = (points[points.Count - 1] - points[points.Count - 2]).magnitude;
        if(lengths.Count > 2)
        {
            lengths[lengths.Count-2] = (points[points.Count-2] - points[points.Count-3]).magnitude;
        }
        CheckMoving();
        CheckCurrent();
        PullPlayer();
        AnimateBar();
    }

    void UpdatePoints()
    {
        points[0] = battery.transform.position;
        points[points.Count - 1] = player.transform.position;
        visualPoints[0] = points[0];
        visualPoints[visualPoints.Count - 1] = points[points.Count - 1];
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, visualPoints[i]);
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
            visualPoints[visualPoints.Count - 1] = hit.point;
            points[points.Count - 1] = newPoint;
            attachedObject[attachedObject.Count - 1] = hit.collider.gameObject;
            visualPoints.Add(player.transform.position);
            points.Add(player.transform.position);
            attachedObject.Add(player);
            line.positionCount++;
            lengths.Add(0f);
            return;
            /*baseLength = 0f;
            for(int i = 0; i < lengths.Count-1; i++)
            {
                baseLength += lengths[i];
            }*/
        }

        //check from battery
        
        direction = (points[1] - points[0]).normalized;
        distance = (points[1] - points[0]).magnitude;
        hit = Physics2D.Raycast(points[0], direction, distance, hittable);
        if (hit.collider != null)
        {
            if (Mathf.Round(hit.point.x * 100) == Mathf.Round(points[1].x * 100) && Mathf.Round(hit.point.y * 100) == Mathf.Round(points[1].y * 100)) { return; }
            Vector2 newPoint = hit.point + hit.normal * outLength;
            visualPoints.Insert(1, newPoint);
            points.Insert(1, newPoint);
            attachedObject.Insert(1, hit.collider.gameObject);
            line.positionCount++;
            lengths.Insert(1, 0f);
        }
    }
    void RemoveTension()
    {
        Vector2 direction = (points[points.Count - 3] - points[points.Count - 1]).normalized;
        float distance = (points[points.Count - 3] - points[points.Count - 1]).magnitude;
        RaycastHit2D hit = Physics2D.Raycast(points[points.Count - 1], direction, distance, hittable);
        //bool pointClose = (hit.point.x + pointTolerence < points[points.Count - 3].x || hit.point.x - pointTolerence > points[points.Count - 3].x) && (hit.point.y + pointTolerence < points[points.Count - 3].y || hit.point.y - pointTolerence > points[points.Count - 3].y);
        if (hit.collider == null)
        {
            for(int i = 1; i < numChecks; i++)
            {
                Vector2 diff = points[points.Count-3] - points[points.Count-2];
                float fraction = i/numChecks;
                Vector2 midPoint = points[points.Count-2] + (fraction * diff);
                Vector2 direction2 = (midPoint - points[points.Count - 1]).normalized;
                float distance2 = (midPoint - points[points.Count - 1]).magnitude;
                RaycastHit2D hit2 = Physics2D.Raycast(points[points.Count - 1], direction2, distance2, hittable);
                bool close = Mathf.Round(hit2.point.x * tolerence) == Mathf.Round(midPoint.x * tolerence) && Mathf.Round(hit2.point.x * tolerence) == Mathf.Round(midPoint.x * tolerence);
                //bool close = (hit2.point.x + tolerence < midPoint.x || hit2.point.x - tolerence > midPoint.x) && (hit2.point.y + tolerence < midPoint.y || hit2.point.y - tolerence > midPoint.y);
                Debug.DrawRay(points[points.Count - 1], direction2 * distance2, Color.red, 2f); //not raycasting to right point :(
                if (hit2.collider != null) { if (!close) { return; } }
            }

            visualPoints[visualPoints.Count - 2] = points[points.Count - 1];
            points[points.Count - 2] = points[points.Count - 1];
            if (attachedObject[attachedObject.Count - 2].GetComponent<MetalBox>() != null) 
            {
                attachedObject[attachedObject.Count - 2].GetComponent<MetalBox>().active = false;
            }
            attachedObject[attachedObject.Count - 2] = attachedObject[attachedObject.Count - 1];
            points.RemoveAt(points.Count-1);
            visualPoints.RemoveAt(visualPoints.Count - 1);
            attachedObject.RemoveAt(attachedObject.Count-1);
            line.positionCount--;
            lengths.RemoveAt(lengths.Count-1);
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
                visualPoints[i] += attachedObject[i].GetComponent<MovingPlatform>().difference;
                lengths[i - 1] = (points[i] - points[i - 1]).magnitude;
                lengths[i] = (points[i + 1] - points[i]).magnitude;
            }
        }
    }
    void CheckCurrent()
    {
        for(int i = 0; i < attachedObject.Count - 1; i++)
        {
            if (attachedObject[i].GetComponent<MetalBox>()  != null)
            {
                attachedObject[i].GetComponent<MetalBox>().active = true; 
            }
        }
    }
    private void ResetPoints()
    {
        for(int i = 1; i < attachedObject.Count - 1; i++)
        {
            if (attachedObject[i].GetComponent<MetalBox>() != null)
            {
                attachedObject[i].GetComponent<MetalBox>().active = false;
            }
        }
        visualPoints.Clear();
        points.Clear();
        attachedObject.Clear();
        lengths.Clear();
        visualPoints.Add(battery.transform.position);
        visualPoints.Add(player.transform.position);
        points.Add(battery.transform.position);
        points.Add(player.transform.position);
        attachedObject.Add(battery);
        attachedObject.Add(player);
        lengths.Add((points[1] - points[0]).magnitude);
        line.positionCount = 2;
    }
    private void AnimateBar()
    {
        int index = (int)Mathf.Clamp((float)((int)Mathf.Round((totalLength / maxLength) * 9f)), 0f, 9f);
        Debug.Log(index);
        bar.sprite = barSprites[index];
    }
}
