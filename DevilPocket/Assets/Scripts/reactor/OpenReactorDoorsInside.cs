using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenReactorDoorsInside : MonoBehaviour {


    public Animator openingDoors;


    bool iceBeaten = false;
    bool metalBeaten = false;
    bool poisonBeaten = false;
    bool godBeaten = false;

    PlayerInventory playerInventory;

    private void Start() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
    }

    
    // 5 functions that wil order the opening of the doors
    public void OpenFirstDoor(List<string> args) {
        Debug.Log("Open deur");
        openingDoors.SetTrigger("First");
        //openingDoors.SetTrigger("Ice");
    }

    public void OpenSecondDoor(List<string> args) {
        if (playerInventory.beatenBosses[(int)Element.Ice]) {
            openingDoors.SetTrigger("Ice");
        }
    }

    public void OpenThirdDoor(List<string> args) {
        if (playerInventory.beatenBosses[(int)Element.Metal]) {
            openingDoors.SetTrigger("Metal");
        }
    }

    public void OpenFourthDoor(List<string> args) {
        if (playerInventory.beatenBosses[(int)Element.Poison]) {
            openingDoors.SetTrigger("Poison");
        }
    }

    public void OpenFifthDoor(List<string> args) {
        if (playerInventory.beatenBosses[(int)Element.Normal]) {
            openingDoors.SetTrigger("God");
        }
    }
}
