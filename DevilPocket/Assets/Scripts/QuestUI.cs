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

    void Start() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        
    }

    private void OnEnable() {
        LoadQuest();
    }

    private void LoadQuest(int desiredIndex = 0) {
        int count = playerInventory.GetAllQuests().Length;

        if (desiredIndex < 0) {
            desiredIndex = count + desiredIndex;
        }

        currentQuest = Mathf.Clamp(desiredIndex, 0, count);

        if (count == 0) {
            LoadEmptyQuest();   
        } else {
            LoadQuest(currentQuest);
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
        claimButton.enabled = false;
        leftCycleButton.enabled = false;
        rightCycleButton.enabled = false;
    }

    public void LoadExistingQuest(int index) {
        int count = playerInventory.GetAllQuests().Length;
        Quest quest = playerInventory.GetAllQuests()[index];

        questIndexText.text = $"Quest {index + 1}/{count}";
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

        questDescriptionText.text = quest.questDescription;
        claimButton.enabled = quest.Completed && !quest.collectedReward;

        leftCycleButton.enabled = count > 1;
        rightCycleButton.enabled = count > 1;
    }

    public void CycleLeft() {
        LoadQuest(currentQuest + 1);
    }

    public void CycleRight() {
        LoadQuest(currentQuest - 1);
    }

    public void ClaimReward() {
        Quest quest = playerInventory.GetAllQuests()[currentQuest];
        quest.collectedReward = true;

        playerInventory.money += quest.moneyReward;
        claimButton.enabled = false;
    }
}
