using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;
public class Puse : MonoBehaviour {
    [SerializeField] private GameObject pausePanel;


    private void Awake() {
        pausePanel.SetActive(false);
    }


    void Update() {
        if (CrossPlatformInputManager.GetButtonDown("Pouse")) {
            if (!pausePanel.activeInHierarchy) {
                PauseGame();
            }
            else if (pausePanel.activeInHierarchy) {
                ContinueGame();
            }
        }
    }


    private void PauseGame() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Debug.Log("pouse");
    }


    private void ContinueGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Debug.Log("Play");
    }
}
