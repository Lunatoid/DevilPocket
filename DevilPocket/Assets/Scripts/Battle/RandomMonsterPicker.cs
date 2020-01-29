using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMonsterPicker : MonoBehaviour {

    [System.Serializable]
    struct MonsterEntry {
        [Header("% chance"), Range(0.0f, 1.0f)]
        public float percentChance;
        public GameObject monsterPrefab;
    }

    [SerializeField]
    List<MonsterEntry> randomMonsters = new List<MonsterEntry>();
    System.Random random = new System.Random();

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    void ShuffleList() {
        // Fisher-Yates shuffle.
        int n = randomMonsters.Count;
        while (n > 1) {
            --n;
            int k = random.Next(n + 1);
            MonsterEntry tmp = randomMonsters[k];
            randomMonsters[k] = randomMonsters[n];
            randomMonsters[n] = tmp;
        }
    }

    /// <summary>
    /// Returns a random monster based on their appearance chance.
    /// </summary>
    /// <returns>A random monster based on their appearance chance.</returns>
    public GameObject GetRandomMonsterPrefab() {
        // Shuffle the list so we don't have any bias related to the position within the array
        ShuffleList();

        foreach (MonsterEntry entry in randomMonsters) {
            if (Random.Range(0.0f, 1.0f) <= entry.percentChance) {
                // The check passed, this is the one.
                return entry.monsterPrefab;
            }
        }

        // If no monster returned, just return the first item in the list, which should still be random thanks to the shuffle
        return randomMonsters[0].monsterPrefab;
    }

    /// <summary>
    /// Looks through all the monsters and returns the prefab of the specified name.
    /// </summary>
    /// <param name="monsterName">Name of the monster.</param>
    /// <returns></returns>
    public GameObject GetMonsterPrefabByName(string monsterName) {
        foreach (MonsterEntry entry in randomMonsters) {
            if (entry.monsterPrefab.GetComponent<Monster>().monsterName == monsterName) {
                return entry.monsterPrefab;
            }
        }

        return null;
    }
}
