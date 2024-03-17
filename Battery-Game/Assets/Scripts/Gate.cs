using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    bool on;
    bool on2;
    public bool active;
    public MetalPad padInput;
    public MetalBox boxInput;
    public Gate gateInput;
    public Wire wireInput;
    public MetalPad padInput2;
    public MetalBox boxInput2;
    public Gate gateInput2;
    public Wire wireInput2;
    // 0 = not | 1 = delay | 2 = and 
    public int type;
    public float delayTime = 1f;
    bool delayGateCounting = false;

    LineRenderer wire;
    public GameObject attachedObject;
    Color32 wireColorOff = new Color32(0x23, 0x1B, 0x23, 0xFF);
    Color32 wireColorOn = new Color32(0xF9, 0xC2, 0x2B, 0xFF);

    public List<Sprite> sprites = new List<Sprite>();
    SpriteRenderer sr;
    public int charge = 0;

    bool sentSpark = false;
    public GameObject sparkPrefab;

    bool playing = false;
    bool fading = false;

    public ParticleSystem particles;

    private void Start()
    {
        wire = GetComponent<LineRenderer>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (active)
        {
            wire.startColor = wireColorOn;
            wire.endColor = wireColorOn;
        }
        else
        {
            wire.startColor = wireColorOff;
            wire.endColor = wireColorOff;
        }

        if (padInput != null)
        {
            on = padInput.active;
        }
        else if(boxInput != null)
        {
            on = boxInput.active;
        }
        else if(gateInput != null)
        {
            on = gateInput.active;
        }
        else if (wireInput != null)
        {
            on = wireInput.active;
        }

        if (padInput2 != null)
        {
            on2 = padInput2.active;
        }
        else if(boxInput2 != null)
        {
            on2 = boxInput2.active;
        }
        else if(gateInput2 != null)
        {
            on2 = gateInput2.active;
        }
        else if (wireInput2 != null)
        {
            on2 = wireInput2.active;
        }

        if (attachedObject != null)
        {
            wire.SetPosition(wire.positionCount - 1, attachedObject.transform.position);
        }


        if (type == 0) 
        {
            active = !on;
            sr.sprite = sprites[active ? 0 : 1];
        }
        else if(type == 1)
        {
            if(on && charge != 6)
            {
                if(!playing)
                {
                    fading = false;
                    GetComponent<AudioSource>().volume = 0.3f;
                    GetComponent<AudioSource>().time = GetComponent<AudioSource>().clip.length * charge / 6;
                    GetComponent<AudioSource>().Play();
                    particles.Play();
                    playing = true;
                }
            }
            if(!on || charge == 6)
            {
                fading = true;
                playing = false;
                particles.Stop();
            }

            if(fading)
            {
                GetComponent<AudioSource>().volume -= 2f * Time.deltaTime;
            }

            if(on && charge != 6 && !delayGateCounting)
            {
                StartCoroutine(ChargeCooldown());
            }
            else if(!on && charge != 0 && !delayGateCounting)
            {
                StartCoroutine(ChargeCooldown());
            }
            if(charge == 6)
            {
                active = true;
            }
            else
            {
                active = false;
            }
            sr.sprite = sprites[charge];
        }
        else if(type == 2)
        {
            if(on && on2)
            {
                active = true;
            }
            else
            {
                active = false;
            }
            sr.sprite = sprites[active ? 1 : 0];
        }

        if (active && !sentSpark)
        {
            StartCoroutine(SendSpark());
        }
    }

    IEnumerator ChargeCooldown()
    {
        delayGateCounting = true;
        yield return new WaitForSeconds(delayTime/6);
        charge += on ? 1 : -1;
        charge = (int)Mathf.Clamp(charge, 0f, 6f);
        delayGateCounting = false;
    }
    IEnumerator SendSpark()
    {
        sentSpark = true;
        Spark spark = GameObject.Instantiate(sparkPrefab, transform.position, Quaternion.identity).GetComponent<Spark>();
        spark.gate = this;
        for (int i = 0; i < wire.positionCount; i++)
        {
            spark.points.Add((Vector2)wire.GetPosition(i));
        }
        if (attachedObject != null) { spark.attachedObject = attachedObject; }
        yield return new WaitForSeconds(1f);
        sentSpark = false;
    }
}
