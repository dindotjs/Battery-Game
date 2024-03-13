using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    List<string> scenes = new List<string>{ "Tutorial1", "Level1", "HintReset", "Level2", "NotTutorial", "AndTutorial", "Level4", "DelayTutorial", "ChargeLatch", "MovingPlatIntro", "Level3", "DoorHop", "Level5", "Level7", "Level6", "BaseLevel", "SampleScene"};
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
            StartCoroutine(LoadScene(currentScene, 0.1f));
        }
    }

    public IEnumerator LoadScene(int index, float time) 
    {
        yield return new WaitForSeconds(time);
        currentScene = index;
        SceneManager.LoadScene(scenes[index]);
    }
}
