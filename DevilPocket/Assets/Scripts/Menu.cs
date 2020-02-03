using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;

public class Menu : MonoBehaviour
{
    LoadTransition lt;

    private void Start() {
        lt = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LoadTransition>();
    }

    public void LoadGame() {
        StartCoroutine(LoadMainSceneCO());
    }

    public void NewGame() {
        if (File.Exists($"{Application.dataPath}/save.txt")) {
            File.Delete($"{Application.dataPath}/save.txt");
        }
        LoadGame();
    }

    public void ExitGame() {
        Debug.Log("Exit app");
        Application.Quit();
    }


    IEnumerator LoadMainSceneCO(){
        lt.FadeToBlack();
        yield return new WaitForSeconds(1.1f);
        AsyncOperation op = SceneManager.LoadSceneAsync("LoadScene");
    }

}
