using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;

public class PlayerInventory : MonoBehaviour, IShopCostumer {

    [SerializeField]
    public GameObject[] carriedMonsters = new GameObject[2];

    public int enemyMonsterLevel = 1;
    public GameObject enemyMonster;

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

    [SerializeField]
    GameObject questLedgerPrefab;
    QuestLedger questLedger;

    [System.Serializable]
    public struct PcEntry {
        public string name;
        public string saveString;
    }

    [HideInInspector]
    public List<PcEntry> pcStorage = new List<PcEntry>();

    public bool wonLastBattle = false;
    Element? currentBossBattle = null; // null == no boss battle
    public bool[] beatenBosses = new bool[4];

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        GameObject ledger = Instantiate(questLedgerPrefab, transform);
        questLedger = ledger.GetComponent<QuestLedger>();

        // Populate our inventory with two random monsters
        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
        
        // Instantiate and initialize them
        carriedMonsters[0] = Instantiate(randomMonsterPicker.GetRandomMonsterPrefab(), gameObject.transform);
        carriedMonsters[0].GetComponent<Monster>().ownedByPlayer = true;
        carriedMonsters[0].SetActive(false);

        for (int i = 1; i < carriedMonsters.Length; ++i) {
            carriedMonsters[i] = null;
        }

        shopSource = GetComponent<AudioSource>();

        SceneManager.activeSceneChanged += CheckForBossCompletion;
    }

    public string sceneBeforeBattle;
    private void CheckForBossCompletion(Scene arg0, Scene arg1) {
        if (arg1.name != "BattleScene") {
            sceneBeforeBattle = arg1.name;
        }

        if (arg1.name == "Example_01" && wonLastBattle && currentBossBattle != null) {
            beatenBosses[(int)currentBossBattle] = true;
            Debug.Log("Player won against " + currentBossBattle);
            currentBossBattle = null;
        }
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

    public void AddQuest(string name) {
        questLedger.AcceptQuest(name);
    }


    /// <summary>
    /// Updates completion on all quests.
    /// </summary>
    /// <param name="intendedType">The intended type of the amount.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>Whether or not any of them completed.</returns>
    public bool UpdateCompletion<T>(GoalType intendedType, int amount = 1, T customData = default) {
        return questLedger.UpdateCompletion(intendedType, amount, customData);
    }

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

    /// <summary>
    /// Adds the monster to the pc.
    /// </summary>
    /// <param name="monster">The monster.</param>
    public void AddMonsterToPc(Monster monster) {
        PcEntry entry;
        entry.name = monster.monsterName;
        entry.saveString = monster.SaveToString();

        pcStorage.Add(entry);
    }

    /// <summary>
    /// Loads the monster from the pc and stores the one it is going to replace.
    /// </summary>
    /// <param name="pcIndex">Index of the monster in the pc.</param>
    /// <param name="secondaryMonster">If <c>false</c> it will replace the primary monster, if <c>true</c> it will replace the secondary.</param>
    public void LoadMonsterFromPc(int pcIndex, bool secondaryMonster = false) {
        int index = (secondaryMonster) ? 1 : 0;
        PcEntry entry = pcStorage[pcIndex];

        GameObject prefab = randomMonsterPicker.GetMonsterPrefabByName(entry.name);

        // Put the current monster in the PC
        if (carriedMonsters[index] != null) {
            AddMonsterToPc(carriedMonsters[index].GetComponent<Monster>());
        }

        // Load the monster
        carriedMonsters[index] = Instantiate(prefab, gameObject.transform);
        carriedMonsters[index].GetComponent<Monster>().LoadFromString(entry.saveString);
        carriedMonsters[index].GetComponent<Monster>().ownedByPlayer = true;
        carriedMonsters[index].SetActive(false);
    }

    public void LoadMonsterIntoParty(Monster monster, bool secondaryMonster = false) {
        int index = (secondaryMonster) ? 1 : 0;

        GameObject prefab = randomMonsterPicker.GetMonsterPrefabByName(monster.monsterName);
        carriedMonsters[index] = Instantiate(prefab, gameObject.transform);
        carriedMonsters[index].GetComponent<Monster>().LoadFromString(monster.SaveToString());
        carriedMonsters[index].GetComponent<Monster>().ownedByPlayer = true;
        carriedMonsters[index].SetActive(false);
    }

    public string SaveQuestLedger() {
        return questLedger.Save();
    }

    public void LoadQuestLedger(StreamReader reader) {
        questLedger.Load(reader);
    }

    public Quest GetQuest(int index) {
        return questLedger.GetQuest(index);
    }

    public int GetQuestCount() {
        return questLedger.GetQuestCount();
    }


    public void StartBossBattle(Element battle) {
        currentBossBattle = battle;

        switch (battle) {
            case Element.Ice:
                enemyMonster = randomMonsterPicker.GetMonsterPrefabByName("Jesus");
                enemyMonsterLevel = 25;
                break;

            case Element.Metal:
                enemyMonster = randomMonsterPicker.GetMonsterPrefabByName("Fernando");
                enemyMonsterLevel = 40;
                break;

            case Element.Poison:
                enemyMonster = randomMonsterPicker.GetMonsterPrefabByName("Saint Nicholas");
                enemyMonsterLevel = 55;
                break;

            case Element.Normal:
                string enemy = (Random.Range(0.0f, 1.0f) >= 0.5f) ? "Joris" : "Tom";
                enemyMonster = randomMonsterPicker.GetMonsterPrefabByName(enemy);
                enemyMonsterLevel = 70;
                break;
        }

        StartCoroutine(StartBattleScene());
    }

    IEnumerator StartBattleScene() {
        GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LoadTransition>().SlideUpDown();
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("BattleScene");
    }
}
