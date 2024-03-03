using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    List<string> scenes = new List<string>{ "SampleScene", "BaseLevel" };
    public int currentScene;

    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("LevelManager").Length > 1)
        {
            GameObject.Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        currentScene = scenes.IndexOf(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LoadScene(currentScene));
        }
    }

    public IEnumerator LoadScene(int index) 
    {
        yield return new WaitForSeconds(0.5f);
        currentScene = index;
        SceneManager.LoadScene(scenes[index]);
    }
}
