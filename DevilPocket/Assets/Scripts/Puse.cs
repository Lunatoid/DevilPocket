using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Puse : MonoBehaviour {

    [SerializeField] private GameObject pausePanel;

    LoadTransition lt;

    public GameObject inventory;
    public GameObject quests;
    public GameObject backgoundPannel;
    private FirstPersonController player;

    public bool ispouse = false;

    public Button firtstSelect;

    private void Awake() {
        pausePanel.SetActive(false);
        inventory.SetActive(false);
        quests.SetActive(false);
        backgoundPannel.SetActive(false);
    }

    private void Start() {
        lt = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LoadTransition>();

        player = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }


    void Update() {
        if (CrossPlatformInputManager.GetButtonDown("Pouse")) {
            if (!pausePanel.activeInHierarchy) {
                PauseGame();
            } else if (pausePanel.activeInHierarchy) {
                ContinueGame();
            }
        }
    }

    private void PauseGame() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.enabled = false;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        ispouse = true;
        firtstSelect.Select();
        Debug.Log("pouse");
    }

    public void ContinueGame() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
        player.enabled = true;
        pausePanel.SetActive(false);
        ispouse = false;
        Debug.Log("Play");
    }

    public void ShowInventory() {
        inventory.SetActive(true);
        quests.SetActive(false);
        backgoundPannel.SetActive(true);
    }

    public void ShowQwests() {
        inventory.SetActive(false);
        quests.SetActive(true);
        backgoundPannel.SetActive(true);
    }

    public void LoadMenuScene() {
        GameObject player = GameObject.Find("Player");
        player.GetComponentInChildren<Saver>().Save();
        StartCoroutine(LoadMenuSceneCO());
    }

    IEnumerator LoadMenuSceneCO() {
        ContinueGame();
        lt.FadeToBlack();

        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("PlayerInventory"));
        Destroy(GameObject.Find("EncounterManager"));
        Destroy(GameObject.Find("RandomMonsterPicker"));

        yield return new WaitForSeconds(1.1f);
        AsyncOperation op = SceneManager.LoadSceneAsync("MenuScene");
    }




}
