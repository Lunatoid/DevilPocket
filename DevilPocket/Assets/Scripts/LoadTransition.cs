using System.Collections;
using UnityEngine;

public class LoadTransition : MonoBehaviour {
    public Animator transitionAnimtor;

    public void SildeUpDouwn() {
        transitionAnimtor.SetTrigger("start");
        Debug.Log("Start anim beattle enter (schreen to black)");
    }

    public void FadeToBlack() {
        transitionAnimtor.SetTrigger("exit");
    }

}
