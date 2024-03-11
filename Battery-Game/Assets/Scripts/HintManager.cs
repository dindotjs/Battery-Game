using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    float showTime = 3f;
    public bool showing = false;
    public List<SpriteRenderer> hintBoxes = new List<SpriteRenderer>();
    public List<int> indexes = new List<int>();
    public List<HintTrigger> hintTriggers = new List<HintTrigger>();
    public int currentBox = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < hintBoxes.Count; i++)
        {
            hintBoxes[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H) && !showing)
        {
            StartCoroutine(ShowBox());
        }
    }

    IEnumerator ShowBox()
    {
        showing = true;
        List<SpriteRenderer> activeBoxes = new List<SpriteRenderer>();
        for(int i = 0; i < indexes.Count; i++)
        {
            if (indexes[i] == currentBox)
            {
                hintBoxes[i].enabled = true;
                activeBoxes.Add(hintBoxes[i]);
            }
        }
        yield return new WaitForSeconds(showTime);
        for(int i = 0; i < activeBoxes.Count; i++)
        {
            activeBoxes[i].enabled = false;
        }
        showing = false;
    }
}
