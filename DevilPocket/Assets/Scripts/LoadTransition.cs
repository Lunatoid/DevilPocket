using System.Collections;
using UnityEngine;

public class LoadTransition : MonoBehaviour {
    public void SlideUpDown() {
        GetComponentInChildren<Animator>().SetTrigger("start");
    }

    public void FadeToBlack() {
        GetComponentInChildren<Animator>().SetTrigger("exit");
    }

}
