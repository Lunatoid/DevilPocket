using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PcUIEntry : MonoBehaviour {
    public TextMeshProUGUI monsterName;
    public TextMeshProUGUI monsterElement;
    public TextMeshProUGUI monsterLevel;
    public Image monsterSprite;
    public int index;

    PlayerInventory playerInventory;
    RandomMonsterPicker randomMonsterPicker;

    // Start is called before the first frame update
    void Start() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
    }

    public void OnEquip() {
        // get player inventory bla bla bla
        bool switched = playerInventory.LoadMonsterFromPc(index, true, index);

        // If we switched, reload this template
        if (switched) {
            PlayerInventory.PcEntry entry = playerInventory.pcStorage[index];

            // Get prefab from name and load relevant data
            GameObject monster = Instantiate(randomMonsterPicker.GetMonsterPrefabByName(entry.name));
            Sprite front = monster.GetComponent<Monster>().sprites[0];
            Element element = monster.GetComponent<Monster>().element;

            // Create dummy monster to parse the data and get the level
            Monster dummy = monster.GetComponent<Monster>();
            dummy.LoadFromString(entry.saveString);
            int level = dummy.monsterLevel;

            Destroy(monster);

            monsterName.text = entry.name;
            monsterLevel.text = $"Lvl. {level}";
            monsterElement.text = element.ToString();
            monsterSprite.sprite = front;
        }
    }
}
