using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBox : MonoBehaviour
{
    public bool active;
    public bool batteryOn = false;
    LineRenderer wire;
    public GameObject attachedObject;
    Color32 wireColorOff = new Color32(0x23, 0x1B, 0x23, 0xFF);
    Color32 wireColorOn = new Color32(0xF9, 0xC2, 0x2B, 0xFF);
    public GameObject sparkPrefab;
    bool sentSpark = false;
    bool flashed = false;
    public List<Sprite> sprites = new List<Sprite>();
    public AudioSource onsfx;
    public ParticleSystem particles;
    public Vector2 particlePos;
    public Vector2 particleNormal;

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
        particles.Stop();
    }

    private void Update()
    {

        if (active)
        {
            wire.startColor = wireColorOn;
            wire.endColor = wireColorOn;
        }
        else
        {
            flashed = false;
            wire.startColor = wireColorOff;
            wire.endColor = wireColorOff;
        }

        if (batteryOn)
        {
            active = true;
        }

        if (attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }

        if(active && !sentSpark)
        {
            StartCoroutine(SendSpark());
        }

        if(active && !flashed)
        {
            StartCoroutine(Flash());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battery" && collision.gameObject.layer != LayerMask.NameToLayer("HeldBattery"))
        {
            particlePos = (Vector2)transform.position + Vector2.up * 0.5f;
            particleNormal = Vector2.up;
            batteryOn = true;
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            batteryOn = false;
            active = false;
        }
    }

    IEnumerator Flash()
    {
        particles.transform.position = particlePos;
        particles.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(Vector2.zero, particleNormal));
        particles.Play();
        flashed = true;
        SoundManager.PlaySoundRandom(onsfx, 0.95f, 1.05f);
        GetComponent<SpriteRenderer>().sprite = sprites[1]; 
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
    IEnumerator SendSpark()
    {
        sentSpark = true; 
        Spark spark = GameObject.Instantiate(sparkPrefab, transform.position, Quaternion.identity).GetComponent<Spark>();
        spark.metalBox = this;
        for(int i = 0; i < wire.positionCount; i++)
        {
            spark.points.Add((Vector2)wire.GetPosition(i));
        }
        if (attachedObject != null) { spark.attachedObject = attachedObject; }
        yield return new WaitForSeconds(1f);
        sentSpark = false;
    }
}
