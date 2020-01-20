using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorDoors : MonoBehaviour
{
    public GameObject doorToIce;

    public GameObject doorToMetal;

    public GameObject doorToPoison;

    public GameObject doorToGod;

    public GameObject doorAfterGod;


    private void Update() {

        if (Input.GetKeyDown(KeyCode.F1)) {
            doorToIce.SetActive(false);
        }
    }

}
