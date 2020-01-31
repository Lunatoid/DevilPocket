﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEBUG!!!
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class PlayerInventory : MonoBehaviour, IShopCostumer {

    [SerializeField]
    GameObject[] carriedMonsters = new GameObject[2];

    public List<GameObject> enemyMonsters = new List<GameObject>(1);

    public int money = 0;

    public bool hasBattlepass = false;
    public int baitAmount = 0;
    public int paracetamolAmount = 0;
    public int ibuprofenAmount = 0;
    public int morphineAmount = 0;

    AudioSource shopSource;

    [SerializeField]
    AudioClip noMoneyClip;

    [SerializeField]
    AudioClip boughtItemClip;

    RandomMonsterPicker randomMonsterPicker;

    List<Quest> quests = new List<Quest>();

    GameObject questHolder;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        questHolder = new GameObject("QuestHolder");
        questHolder.transform.parent = transform;

        // Populate our inventory with two random monsters
        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
        
        // Instantiate and initialize them
        carriedMonsters[0] = Instantiate(randomMonsterPicker.GetRandomMonsterPrefab(), gameObject.transform);
        carriedMonsters[0].GetComponent<Monster>().ownedByPlayer = true;
        carriedMonsters[0].SetActive(false);

        for (int i = 1; i < carriedMonsters.Length; ++i) {
            carriedMonsters[i] = null;
        }

        //for (int i = 0; i < carriedMonsters.Length; ++i) {
        //    if (carriedMonsters[i]) {
        //        carriedMonsters[i] = Instantiate(carriedMonsters[i], gameObject.transform);
        //        carriedMonsters[i].GetComponent<Monster>().ownedByPlayer = true;
        //        carriedMonsters[i].SetActive(false);
        //
        //        // DontDestroyOnLoad(carriedMonsters[i]);
        //    }
        //}

        shopSource = GetComponent<AudioSource>();
    }

    public void LoadMonster(string monsterName, string saveString, bool secondaryMonster = false) {
        int index = (secondaryMonster) ? 1 : 0;

        // Destroy the carried monster
        Destroy(carriedMonsters[index]);

        // Get the monster by name
        carriedMonsters[index] = Instantiate(randomMonsterPicker.GetMonsterPrefabByName(monsterName), gameObject.transform);
        carriedMonsters[index].GetComponent<Monster>().LoadFromString(saveString);
        carriedMonsters[index].SetActive(false);
    }

    /// <summary>
    /// Returns one of the carried monsters of the player.
    /// </summary>
    /// <param name="secondaryMonster">If <c>false</c> it will return the primary monster, if <c>true</c> it will return the secondary.</param>
    /// <returns>One of the carried monsters of the player. Return value will be <c>null</c> if the monster doesn't exist.</returns>
    public GameObject GetMonster(bool secondaryMonster = false) {
        int index = (secondaryMonster) ? 1 : 0;
        return carriedMonsters[index];
    }

    /// <summary>
    /// Swaps the first carried monster with the second one.
    /// </summary>
    public void SwitchMonsters() {
        GameObject tmp = carriedMonsters[0];
        carriedMonsters[0] = carriedMonsters[1];
        carriedMonsters[1] = tmp;
    }

    public void AddQuest(Quest quest) {
        Quest newQuest = questHolder.AddComponent<Quest>();
        newQuest.CopyFromQuest(quest);

        quests.Add(newQuest);
    }


    /// <summary>
    /// Updates completion on all quests.
    /// </summary>
    /// <param name="intendedType">The intended type of the amount.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>Whether or not any of them completed.</returns>
    public bool UpdateCompletion<T>(GoalType intendedType, int amount = 1, T customData = default) {
        bool anyCompleted = false;
        foreach (Quest quest in quests) {
            anyCompleted |= quest.UpdateCompletion(intendedType, amount, customData);
        }

        return anyCompleted;
    }

#if UNITY_EDITOR
    private void Update() {
        // DEBUG!!!
        if (Input.GetKeyDown(KeyCode.F10)) {
            SceneManager.LoadScene("BattleScene");
        }
    }
#endif

    public void BoughtItem(Item.ItemType itemType) {
        Debug.Log("Bought item" + itemType);
        switch (itemType) {

            // when inplemented add the item on the corospending line.
            case Item.ItemType.Aas:             baitAmount++; break;
            case Item.ItemType.Paracetamol:     paracetamolAmount++; break;
            case Item.ItemType.Ibuprofen:       ibuprofenAmount++; break;
            case Item.ItemType.Morphine:        morphineAmount++; break;
            case Item.ItemType.BattlePass:      hasBattlepass = true; break;
        }
    }

    public bool TrySpendGoldAmount(int spendGoldAmount) {
        if (money >= spendGoldAmount) {
            money -= spendGoldAmount;
            shopSource.clip = boughtItemClip;
            shopSource.Play();
            return true;
        } else {
            shopSource.clip = noMoneyClip;
            shopSource.Play();
            return false;
        }
    }
}
