using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsLoad : MonoBehaviour
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

        yield return new WaitForSeconds(1.1f);
        lt.FadeToBlack();
        yield return new WaitForSeconds(1.1f);

        SceneManager.LoadScene("Credits");
    }

}
