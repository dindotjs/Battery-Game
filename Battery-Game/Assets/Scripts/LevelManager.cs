using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    List<string> scenes = new List<string>{ "LevelSelect", "Tutorial1", "Level1", "HintReset", "Level2", "NotTutorial", "AndTutorial", "Level4", "DelayTutorial", "ChargeLatch", "MovingPlatIntro", "Level3", "DoorHop", "Level11", "Level5", "Level10", "Level8", "Level7", "Level9", "Level6", "EndLevel"};
    public int currentScene;
    public FadeManager fadeManager;
    public bool loadingScene = false;

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
            StartCoroutine(LoadScene(currentScene, 0.8f));
        }
    }

    public IEnumerator LoadScene(int index, float time) 
    {
        if(loadingScene) { yield break; }
        loadingScene = true;
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager").GetComponent<FadeManager>();
        if (fadeManager != null)
        {
            if(fadeManager.fadingOut) { loadingScene = false;  yield break; }
            StartCoroutine(fadeManager.FadeOut());
        }
        yield return new WaitForSeconds(time);
        currentScene = index;
        loadingScene = false;
        SceneManager.LoadScene(scenes[index]);
    }
}
