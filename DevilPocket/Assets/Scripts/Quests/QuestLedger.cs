using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class QuestLedger : MonoBehaviour {

    [SerializeField]
    Quest[] quests;

    void Start() {
        // See Monster to why this is being done
        for (int i = 0; i < quests.Length; ++i) {
            Quest instantiated = gameObject.AddComponent<Quest>();
            instantiated.CopyFromQuest(quests[i]);
            instantiated.questId = quests[i].name;
            quests[i] = instantiated;
        }
    }

    Quest GetQuestFromName(string name) {
        for (int i = 0; i < quests.Length; ++i) {
            if (quests[i].questId == name) {
                return quests[i];
            }
        }

        return null;
    }

    public void AcceptQuest(string name) {
        GetQuestFromName(name).acceptedQuest = true;
    }

    /// <summary>
    /// Updates completion on all quests.
    /// </summary>
    /// <param name="intendedType">The intended type of the amount.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>Whether or not any of them completed.</returns>
    public bool UpdateCompletion<T>(GoalType intendedType, int amount = 1, T customData = default) {
        bool anyCompleted = false;

        foreach (Quest q in quests) {
            anyCompleted |= q.UpdateCompletion(intendedType, amount, customData);
        }

        return anyCompleted;
    }

    // Start is called before the first frame update
    public string Save() {
        string saveString = "";

        for (int i = 0; i < quests.Length; ++i) {
            saveString += quests[i].SaveToString();

            if (i + 1 < quests.Length) {
                saveString += "\n";
            }
        }

        return saveString;
    }

    // Update is called once per frame
    public void Load(StreamReader reader) {
        for (int i = 0; i < quests.Length; ++i) {
            quests[i].LoadFromString(reader.ReadLine());
        }
    }

    public Quest GetQuest(int index) {
        return quests[index];
    }

    public int GetQuestCount() {
        return quests.Length;
    }
}
