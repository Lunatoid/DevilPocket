using System.Collections;
using UnityEngine;

public class LoadTransition : MonoBehaviour {
    public Animator transitionAnimtor;

    void Start() {
        transitionAnimtor = GetComponentInChildren<Animator>();
    }

    public void SildeUpDouwn() {
        if (!transitionAnimtor) return;

        transitionAnimtor.SetTrigger("start");
        Debug.Log("Start anim beattle enter (schreen to black)");
    }

    public void FadeToBlack() {
        if (!transitionAnimtor) return;
        transitionAnimtor.SetTrigger("exit");
    }

}
