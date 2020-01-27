using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {

    public string questGiverName;

    public Quest quest;

    bool alreadyGaveQuest = false;

    private void Start() {
        quest.giver = this;
    }

    // Meant to be called with #function
    void GiveQuest(List<string> args) {
        if (!alreadyGaveQuest) {
            alreadyGaveQuest = true;
            GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>().AddQuest(quest);
        }
    }
}
