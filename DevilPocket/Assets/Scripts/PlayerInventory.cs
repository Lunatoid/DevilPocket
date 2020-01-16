using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEBUG!!!
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class PlayerInventory : MonoBehaviour {
    
    [SerializeField]
    GameObject[] carriedMonsters = new GameObject[2];

    public List<GameObject> enemyMonsters = new List<GameObject>();

    public int money = 0;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

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

#if UNITY_EDITOR
    private void Update() {
        // DEBUG!!!
        if (Input.GetKeyDown(KeyCode.F10)) {
            SceneManager.LoadScene("BattleScene");
        }
    }
#endif
}
