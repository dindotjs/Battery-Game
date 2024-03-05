using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    HintManager hintManager;

    void Start()
    {
        hintManager = GameObject.FindGameObjectWithTag("HintManager").GetComponent<HintManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hintManager != null)
        {
            if (hintManager.currentBox >= hintManager.hintTriggers.Count) { return; }
            if(other.gameObject.tag == "Player" && hintManager.hintTriggers[hintManager.currentBox] == GetComponent<HintTrigger>())
            {
                hintManager.currentBox++;
            }
        }
    }

}
