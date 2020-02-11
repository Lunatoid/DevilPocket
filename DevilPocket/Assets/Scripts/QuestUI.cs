using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class QuestUI : MonoBehaviour {

    public TextMeshProUGUI questIndexText;
    public TextMeshProUGUI questGiverText;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questRewardText;
    public TextMeshProUGUI questTargetText;
    public TextMeshProUGUI questDescriptionText;
    public Slider questProgressBar;
    public Button claimButton;
    public Button leftCycleButton;
    public Button rightCycleButton;

    int currentQuest = 0;

    PlayerInventory playerInventory;

    public AudioSource sourece;
    public AudioClip calimSound;

    void Awake() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();

    }

    private void OnEnable() {
        LoadQuest(currentQuest);
    }

    int GetAcceptedCount() {
        int count = playerInventory.GetQuestCount();
        int acceptedCount = 0;
        for (int i = 0; i < count; ++i) {
            if (playerInventory.GetQuest(i).acceptedQuest) {
                ++acceptedCount;
            }
        }

        return acceptedCount;
    }

    bool IsQuestAccepted(int index) {
        return playerInventory.GetQuest(index).acceptedQuest;
    }

    int GetAcceptedIndex(int realIndex) {
        int count = playerInventory.GetQuestCount();
        int acceptedCount = 0;
        for (int i = 0; i < realIndex; ++i) {
            if (playerInventory.GetQuest(i).acceptedQuest) {
                ++acceptedCount;
            }
        }

        return acceptedCount;
    }

    private void LoadQuest(int desiredIndex = 0) {
        int count = playerInventory.GetQuestCount();

        bool findPositive = desiredIndex >= currentQuest;

        if (desiredIndex < 0) {
            desiredIndex = count + desiredIndex;
        } else {
            desiredIndex %= count;
        }

        // Find quest that is accepted
        if (!playerInventory.GetQuest(desiredIndex).acceptedQuest) {
            int originalIndex = desiredIndex;
            bool found = false;
            do {
                desiredIndex += (findPositive) ? 1 : -1;

                if (desiredIndex < 0) {
                    desiredIndex = count + desiredIndex;
                } else {
                    desiredIndex %= count;
                }

                if (IsQuestAccepted(desiredIndex)) {
                    found = true;
                    break;
                }
            } while (desiredIndex != originalIndex);

            if (!found) {
                LoadEmptyQuest();
                return;
            }
        }


        if (count == 0) {
            LoadEmptyQuest();
        } else {
            currentQuest = desiredIndex;
            LoadExistingQuest(desiredIndex);
        }
    }

    public void LoadEmptyQuest() {
        questIndexText.text = "";
        questGiverText.text = "";
        questTitleText.text = "No Quests";
        questRewardText.text = "";
        questTargetText.text = "";
        questProgressBar.maxValue = 0;
        questProgressBar.value = 0;
        questDescriptionText.text = "";

        claimButton.interactable = false;
        claimButton.GetComponentInChildren<TextMeshProUGUI>().text = "No Quest";
        ;

        leftCycleButton.interactable = false;
        rightCycleButton.interactable = false;
    }

    public void LoadExistingQuest(int index) {
        int count = playerInventory.GetQuestCount();
        Quest quest = playerInventory.GetQuest(index);

        questIndexText.text = $"Quest {GetAcceptedIndex(index) + 1}/{GetAcceptedCount()}";
        questGiverText.text = quest.questGiverName;
        questTitleText.text = quest.questName;
        questRewardText.text = $"${quest.moneyReward}";

        string monsterName = "";
        switch (quest.goalType) {
            case GoalType.BuyItems:
                questTargetText.text = $"Buy: {quest.buyItemsData.itemType}";

                questProgressBar.maxValue = quest.buyItemsData.amountRequired;
                questProgressBar.value = quest.buyItemsData.amountDone;

                break;

            case GoalType.CatchMonsters:
                monsterName = quest.catchMonstersData.catchName;
                questTargetText.text = $"Catch: {((monsterName.Length > 0) ? monsterName : "Any") }";

                questProgressBar.maxValue = quest.catchMonstersData.amountRequired;
                questProgressBar.value = quest.catchMonstersData.amountDone;

                break;

            case GoalType.KillMonsters:
                monsterName = quest.killMonstersData.killName;
                questTargetText.text = $"Kill: {((monsterName.Length > 0) ? monsterName : "Any") }";

                questProgressBar.maxValue = quest.killMonstersData.amountRequired;
                questProgressBar.value = quest.killMonstersData.amountDone;

                break;
        }

        questTargetText.text += $" ({questProgressBar.maxValue})";

        questDescriptionText.text = quest.questDescription;

        claimButton.interactable = quest.Completed && !quest.collectedReward;

        TextMeshProUGUI buttonText = claimButton.GetComponentInChildren<TextMeshProUGUI>();
        if (quest.Completed) {
            buttonText.text = (quest.collectedReward) ? "Completed!" : "Claim Reward";
        } else {
            buttonText.text = "Not yet completed";
        }

        leftCycleButton.interactable = count > 1;
        rightCycleButton.interactable = count > 1;
    }

    public void CycleLeft() {
        LoadQuest(currentQuest - 1);
    }

    public void CycleRight() {
        LoadQuest(currentQuest + 1);
    }

    public void ClaimReward() {
        sourece.clip = calimSound;
        sourece.Play();
        Quest quest = playerInventory.GetQuest(currentQuest);
        quest.collectedReward = true;

        playerInventory.money += quest.moneyReward;
        claimButton.interactable = false;
    }
}
