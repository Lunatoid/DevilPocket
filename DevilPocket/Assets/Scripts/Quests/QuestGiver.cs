using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {

    // Meant to be called with #function
    void GiveQuest(List<string> args) {
        PlayerInventory playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        foreach (string questName in args) {
            playerInventory.AddQuest(questName);
        }
    }
}
