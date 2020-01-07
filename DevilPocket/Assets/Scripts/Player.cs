using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    // Start is called before the first frame update
    void Awake() {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += TogglePlayer;
    }

    private void TogglePlayer(Scene arg0, Scene arg1) {
        gameObject.SetActive(arg1.name == "MainScene");
    }
}
