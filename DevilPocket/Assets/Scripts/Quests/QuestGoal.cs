using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data for each quest
[System.Serializable]
public struct KillMonstersData {
    public int amountDone;
    public int amountRequired;

    [Header("Leave blank for any monster (NYI)")]
    public string killName; // @TODO: Not yet implemented
}

[System.Serializable]
public struct CatchMonstersData {
    public int amountDone;
    public int amountRequired;

    [Header("Leave blank for any monster (NYI)")]
    public string catchName; // @TODO: Not yet implemented
}

[System.Serializable]
public struct DeliverItemsData {
    // @TODO: ItemType type;
    public int amountDone;
    public int amountRequired;
}

[System.Serializable]
public class QuestGoal {

    public enum GoalType {
        KillMonsters,
        CatchMonsters,
        DeliverItems
    }

    public GoalType goalType;

    public string goalName;

    [Multiline]
    public string goalDescription;

    public KillMonstersData killMonstersData;
    public CatchMonstersData catchMonstersData;
    public DeliverItemsData deliverItemsData;

    public bool Completed {
        get {
            switch (goalType) {
                case GoalType.KillMonsters:
                    return killMonstersData.amountDone >= killMonstersData.amountRequired;

                case GoalType.CatchMonsters:
                    return catchMonstersData.amountDone >= catchMonstersData.amountRequired;

                case GoalType.DeliverItems:
                    return deliverItemsData.amountDone >= deliverItemsData.amountRequired;
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
    public bool UpdateCompletion(GoalType intendedType, int amount = 1) {
        if (intendedType != goalType) return false;

        switch (goalType) {
            case GoalType.KillMonsters:
                killMonstersData.amountDone += amount;
                return Completed;

            case GoalType.CatchMonsters:
                catchMonstersData.amountDone += amount;
                return Completed;

            case GoalType.DeliverItems:
                deliverItemsData.amountDone += amount;
                return Completed;
        }

        return false;
    }
}
