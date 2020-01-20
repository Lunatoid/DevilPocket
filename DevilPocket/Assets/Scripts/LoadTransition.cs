using System.Collections;
using UnityEngine;

public class LoadTransition : MonoBehaviour {
    public Animator transitionAnimtor;

    public void LoadBattle() {
        transitionAnimtor.SetTrigger("start");
        Debug.Log("Start anim beattle enter (schreen to black)");
    }
}
