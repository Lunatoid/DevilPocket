﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEBUG!!!
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class PlayerInventory : MonoBehaviour {
    
    [SerializeField]
    GameObject[] carriedMonsters = new GameObject[2];

    public List<GameObject> enemyMonsters = new List<GameObject>(1);

    public int money = 0;

    public bool hasBattlepass = false;

    List<Quest> quests = new List<Quest>();

    GameObject questHolder;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        questHolder = new GameObject("QuestHolder");
        questHolder.transform.parent = transform;

        // Populate our inventory with two random monsters
        RandomMonsterPicker randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
        for (int i = 0; i < carriedMonsters.Length; ++i) {
            do {
                GameObject candidate = randomMonsterPicker.GetRandomMonsterPrefab();

                for (int j = 0; j < i; ++j) {
                    // Check if we already have this prefab
                    if (carriedMonsters[j] == candidate) {
                        continue;
                    }
                }

                // This one is unique, add it
                carriedMonsters[i] = candidate;
                break;
            } while (true);
        }

        // Instantiate and initialize them
        for (int i = 0; i < carriedMonsters.Length; ++i) {
            if (carriedMonsters[i]) {
                carriedMonsters[i] = Instantiate(carriedMonsters[i], gameObject.transform);
                carriedMonsters[i].GetComponent<Monster>().ownedByPlayer = true;
                carriedMonsters[i].SetActive(false);
        
                // DontDestroyOnLoad(carriedMonsters[i]);
            }
        }

        
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
}
