using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour {

    public string questGiverName;

    public Quest quest;

    // Meant to be called with #function
    void GiveQuest(List<string> args) {
        GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>().AddQuest(this, quest);
    }
}
