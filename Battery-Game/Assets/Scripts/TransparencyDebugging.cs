using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyDebugging : MonoBehaviour
{
    bool gettingLighter = true;
    SpriteRenderer sr;
    float transparency = 1f;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gettingLighter)
        {
            transparency -= 1f * Time.deltaTime;
        }
        else
        {
            transparency += 1f * Time.deltaTime;
        }
        if (transparency < 0f)
        {
            gettingLighter = false;
        }
        else if (transparency > 1f)
        {
            gettingLighter = true;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, transparency);
    }
}
