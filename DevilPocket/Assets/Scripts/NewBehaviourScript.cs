using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    LoadTransition lt;
    // Start is called before the first frame update
    private void Start() {
        lt = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LoadTransition>();
    }

    public void End() {
        StartCoroutine(EndEnd());
    }

    IEnumerator EndEnd() {

        lt.FadeToBlack();
        yield return new WaitForSeconds(1.1f);

        SceneManager.LoadScene("MenuScene");
    }
}
