using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorEntrence : MonoBehaviour {
    public Animator openingDoors;

    PlayerInventory playerInventory;

    private void Start() {

        openingDoors = GetComponent<Animator>();

        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventory>();

    }

    public void OnTriggerEnter(Collider reactorDoorTrigger) {

        Debug.Log("enterd the reactor entrace");


        if (playerInventory.hasBattlepass == true) {

            Debug.Log("The reactor opens");
            openingDoors.SetTrigger("hasBattlePass");
        }

    }
}
