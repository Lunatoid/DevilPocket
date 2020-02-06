using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject playermonster1;

    private void Start() {
       // playermonster1.SetActive(false);  
    }

    public void StepOneDone() {
        playermonster1.SetActive(true);
    }

    

}
