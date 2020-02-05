using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data for each quest
[System.Serializable]
public struct KillMonstersData {
    public int amountDone;
    public int amountRequired;

    [Header("Leave blank for any monster")]
    public string killName;
}

[System.Serializable]
public struct CatchMonstersData {
    public int amountDone;
    public int amountRequired;

    [Header("Leave blank for any monster")]
    public string catchName;
}

[System.Serializable]
public struct BuyItemsData {
    public Item.ItemType itemType;
    public int amountDone;
    public int amountRequired;
}

public enum GoalType {
    KillMonsters,
    CatchMonsters,
    BuyItems
}

public class Quest : MonoBehaviour {
    public GoalType goalType;

    [HideInInspector]
    public string questId;

    public string questGiverName;
    public string questName;

    public bool acceptedQuest = false;

    [Multiline]
    public string questDescription;

    public int moneyReward;
    public bool collectedReward = false;

    public KillMonstersData killMonstersData;
    public CatchMonstersData catchMonstersData;
    public BuyItemsData buyItemsData;

    public bool Completed {
        get {
            switch (goalType) {
                case GoalType.KillMonsters:
                    return killMonstersData.amountDone >= killMonstersData.amountRequired;

                case GoalType.CatchMonsters:
                    return catchMonstersData.amountDone >= catchMonstersData.amountRequired;

                case GoalType.BuyItems:
                    return buyItemsData.amountDone >= buyItemsData.amountRequired;
            }
            
            return false;
        }
    }

    /// <summary>
    /// Updates the completion amount if the intended type is the same.
    /// </summary>
    /// <param name="intendedType">The intended type of the amount.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>Whether the goal was completed.</returns>
    public bool UpdateCompletion<T>(GoalType intendedType, int amount = 1, T customData = default) {
        if (intendedType != goalType) return false;

        switch (goalType) {
            case GoalType.KillMonsters:
                if (customData is string) {
                    // This is the string of the monster name
                    if (killMonstersData.killName.Length > 0 && customData as string != killMonstersData.killName) break;
                }

                killMonstersData.amountDone += amount;
                break;

            case GoalType.CatchMonsters:
                if (customData is string) {
                    // This is the string of the monster name
                    if (catchMonstersData.catchName.Length > 0 && customData as string != catchMonstersData.catchName) break;
                }

                catchMonstersData.amountDone += amount;
                break;

            case GoalType.BuyItems:
                if (customData is Item.ItemType) {
                    // This is the item to collect
                    Item.ItemType itemType = (Item.ItemType)(System.Object)customData;
                    if (itemType != buyItemsData.itemType) break;
                }

                buyItemsData.amountDone += amount;
                break;
        }

        return Completed;
    }

    public void CopyFromQuest(Quest other) {
        questGiverName = other.questGiverName;
        questName = other.questName;
        questDescription = other.questDescription;
        acceptedQuest = other.acceptedQuest;

        goalType = other.goalType;
        killMonstersData = other.killMonstersData;
        catchMonstersData = other.catchMonstersData;
        buyItemsData = other.buyItemsData;

        moneyReward = other.moneyReward;
    }

    public string SaveToString() {
        return $"{acceptedQuest},{collectedReward},{killMonstersData.amountDone},{catchMonstersData.amountDone},{buyItemsData.amountDone}";
    }

    public void LoadFromString(string saveString) {
        string[] lines = saveString.Split(',');

        Debug.Assert(lines.Length == 5);

        acceptedQuest = bool.Parse(lines[0]);
        collectedReward = bool.Parse(lines[1]);
        killMonstersData.amountDone = int.Parse(lines[2]);
        catchMonstersData.amountDone = int.Parse(lines[3]);
        buyItemsData.amountDone = int.Parse(lines[4]);
    }
}
