using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBattle : MonoBehaviour {

    [SerializeField]
    Element bossBattle;

    PlayerInventory playerInventory;

    public void Start() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();

        if (playerInventory.beatenBosses[(int)bossBattle]) {
            Destroy(gameObject);
        }
    }

    void StartBossBattle() {
        playerInventory.StartBossBattle(bossBattle);
    }

}
