using System.Collections;
using UnityEngine;

public class LoadTransition : MonoBehaviour {
    public Animator transitionAnimtor;

    public IEnumerator LoadBattle() {

        transitionAnimtor.SetTrigger("start");

        yield return new WaitForSeconds(1.1f);
    }
}
