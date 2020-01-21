using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadReactor : MonoBehaviour {

    LoadTransition lt;

    [SerializeField]
    bool inReactor = false;

    private void Start() {
        lt = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LoadTransition>();
    }

    public void OnTriggerEnter(Collider reactorEntrance) {


        if (!inReactor) {
            Debug.Log("Entering the arena");
            lt.FadeToBlack();
            StartCoroutine(EnteringReactor());
        }
        else {
            Debug.Log("Exiting the arena");
            lt.FadeToBlack();
            StartCoroutine(ExitReactor());
        }
    }

    IEnumerator ExitReactor() {

        yield return new WaitForSeconds(1.1f);

        SceneManager.LoadScene("MainScene");

        inReactor = false;
    }

    IEnumerator EnteringReactor() {

        yield return new WaitForSeconds(1.1f);

        SceneManager.LoadScene("Example_01");

        inReactor = true;
    }

}
