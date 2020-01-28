﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data for each quest
[System.Serializable]
public struct KillMonstersData {
    [HideInInspector]
    public int amountDone;
    public int amountRequired;

    [Header("Leave blank for any monster")]
    public string killName;
}

[System.Serializable]
public struct CatchMonstersData {
    [HideInInspector]
    public int amountDone;
    public int amountRequired;

    [Header("Leave blank for any monster")]
    public string catchName;
}

[System.Serializable]
public struct BuyItemsData {
    public Item.ItemType itemType;
    [HideInInspector]
    public int amountDone;
    public int amountRequired;
}

public enum GoalType {
    KillMonsters,
    CatchMonsters,
    BuyItems
}

[System.Serializable]
public class QuestGoal {

    public GoalType goalType;

    public string goalName;

    [Multiline]
    public string goalDescription;

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

    public void CopyFromQuestGoal(QuestGoal other) {
        goalType = other.goalType;
        goalName = other.goalName;
        goalDescription = other.goalDescription;
        killMonstersData = other.killMonstersData;
        catchMonstersData = other.catchMonstersData;
        buyItemsData = other.buyItemsData;
    }
}
