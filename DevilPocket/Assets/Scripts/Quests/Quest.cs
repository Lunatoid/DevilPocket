using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour {

    [HideInInspector]
    public QuestGiver giver;

    public string questName;

    [Multiline]
    public string questDescription;

    public QuestGoal[] questGoals;

    [HideInInspector]
    public int currentGoal;

    public int moneyReward;

    public bool Completed {
        get {
            foreach (QuestGoal goal in questGoals) {
                if (!goal.Completed) {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Updates completion on all goals.
    /// </summary>
    /// <param name="intendedType">The intended type of the amount.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>Whether or not any of them completed.</returns>
    public bool UpdateCompletion<T>(GoalType intendedType, int amount = 1, T customData = default) {
        bool anyCompleted = false;
        foreach (QuestGoal goal in questGoals) {
            anyCompleted |= goal.UpdateCompletion(intendedType, amount, customData);
        }

        // Check if the current quest is completed
        if (questGoals[currentGoal].Completed) {
            ++currentGoal;
        }

        return anyCompleted;
    }
}
