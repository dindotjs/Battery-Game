using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    List<string> scenes = new List<string>{ "Tutorial1", "Level1", "HintReset", "Level2", "NotTutorial", "AndTutorial", "Level4", "DelayTutorial", "ChargeLatch", "MovingPlatIntro", "Level3", "DoorHop", "Level5", "Level6", "BaseLevel", "SampleScene"};
    public int currentScene;

    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("LevelManager").Length > 1)
        {
            Destroy(gameObject);
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
        yield return new WaitForSeconds(0.1f);
        currentScene = index;
        SceneManager.LoadScene(scenes[index]);
    }
}
