using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    List<string> scenes = new List<string>{ "SampleScene", "Scene1" };

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LoadScene(0));
        }
    }

    public IEnumerator LoadScene(int index) 
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scenes[index]);
    }
}
