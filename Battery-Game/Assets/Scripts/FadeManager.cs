using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public GameObject fadeCircle;
    SpriteRenderer fadeSR;
    Animator anim;
    public List<SpriteRenderer> borders = new List<SpriteRenderer>();
    float transparency = 0f;
    float fadeRate = 50f;
    float fadeTime = 0.025f;
    Transform player;
    public bool fadingIn = false;
    public bool fadingOut = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fadeSR = fadeCircle.GetComponent<SpriteRenderer>();
        anim = fadeCircle.GetComponent<Animator>();
        StartCoroutine(FadeIn());
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }

    public IEnumerator TransparencyFadeIn()
    {
        transparency = 0f;
        while(transparency < 1f)
        {
            transparency += fadeRate * Time.deltaTime;
            fadeSR.color = new Color(fadeSR.color.r, fadeSR.color.g, fadeSR.color.b, transparency);
            for(int i = 0; i < borders.Count; i++)
            {
                borders[i].color = new Color(borders[i].color.r, borders[i].color.g, borders[i].color.b, transparency);
            }
            yield return new WaitForSeconds(fadeTime);
        }
        transparency = 1f;
        fadingIn = false;
    }

    public IEnumerator TransparencyFadeOut()
    {
        transparency = 1f;
        while (transparency > 0f)
        {
            transparency -= fadeRate * Time.deltaTime;
            fadeSR.color = new Color(fadeSR.color.r, fadeSR.color.g, fadeSR.color.b, transparency);
            for (int i = 0; i < borders.Count; i++)
            {
                borders[i].color = new Color(borders[i].color.r, borders[i].color.g, borders[i].color.b, transparency);
            }
            yield return new WaitForSeconds(fadeTime);
        }
        transparency = 0f;
        fadingOut = false;
    }

    public IEnumerator FadeIn()
    {
        fadingOut = true;
        anim.Play("closing");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TransparencyFadeOut());
    }

    public IEnumerator FadeOut()
    {
        fadingIn = true;
        anim.Play("open");
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(TransparencyFadeIn());
        yield return new WaitForSeconds(0.24f);
        anim.Play("opening");
    }
}
