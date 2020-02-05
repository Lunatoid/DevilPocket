﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class Puse : MonoBehaviour {

    [SerializeField] private GameObject pausePanel;

    LoadTransition lt;

    public GameObject inventory;
    public GameObject quests;
    public GameObject backgoundPannel;

    public bool ispouse = false;

    private void Awake() {
        pausePanel.SetActive(false);
        inventory.SetActive(false);
        quests.SetActive(false);
        backgoundPannel.SetActive(false);
    }


    void Update() {
        if (CrossPlatformInputManager.GetButtonDown("Pouse")) {
            if (!pausePanel.activeInHierarchy) {
                PauseGame();
                ispouse = true;
            }
            else if (pausePanel.activeInHierarchy) {
                ContinueGame();
                ispouse = false;
            }
        }
    }

    private void PauseGame() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Debug.Log("pouse");
    }

    public void ContinueGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Debug.Log("Play");
    }

    public void LoadMenuScene() {
        StartCoroutine(LoadMenuSceneCO());
    }

    IEnumerator LoadMenuSceneCO() {
        lt.FadeToBlack();
        yield return new WaitForSeconds(1.1f);
        AsyncOperation op = SceneManager.LoadSceneAsync("MenuScene");
    }


    

}
