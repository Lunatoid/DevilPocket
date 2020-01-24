using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenReactorDoorsInside : MonoBehaviour {


    public Animator openingDoors;


    bool iceBeaten = false;
    bool metalBeaten = false;
    bool poisonBeaten = false;
    bool godBeaten = false;

    private void Start() {
        //openingDoors = GetComponent<Animator>();
    }

    
    // 5 functions that wil order the opening of the doors
    public void OpenFirstDoor(List<string> args) {
        Debug.Log("Open deur");
        openingDoors.SetTrigger("First");
        // openingDoors.SetTrigger("Ice");
    }

    public void OpenSecondDoor(List<string> args) {
        if (iceBeaten == true) {
            openingDoors.SetTrigger("Ice");
        }
    }

    public void OpenThirdDoor(List<string> args) {
        if (metalBeaten == true) {
            openingDoors.SetTrigger("Metal");
        }
    }

    public void OpenForthDoor(List<string> args) {
        if (poisonBeaten == true) {
            openingDoors.SetTrigger("Poison");
        }
    }

    public void OpenFifthDoor(List<string> args) {
        if (godBeaten == true) {
            openingDoors.SetTrigger("God");
        }
    }
}
