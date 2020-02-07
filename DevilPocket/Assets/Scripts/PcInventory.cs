using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class PcInventory : MonoBehaviour {
    //Player monsters UI 2x

    [Header("Monster 1")]
    [Space]
    [Header("Player monster UI")]
    [SerializeField] private TextMeshProUGUI playerMonstername1;
    [SerializeField] private Image playerMonsterSprite1;
    [SerializeField] private TextMeshProUGUI playerMonsterType1;
    [SerializeField] private TextMeshProUGUI playerMonsterLVL1;
    [SerializeField] public Button storeMonster1;

    [Header("Monster 2")]
    [SerializeField] private TextMeshProUGUI playerMonstername2;
    [SerializeField] private Image playerMonsterSprite2;
    [SerializeField] private TextMeshProUGUI playerMonsterType2;
    [SerializeField] private TextMeshProUGUI playerMonsterLVL2;
    [SerializeField] public Button storeMonster2;

    [Space]

    [SerializeField] private GameObject uiTemplatePrefab;
    [SerializeField] private GameObject holder;

    //Buttons navigation 2x

    [Header("Navigation Buttons")]
    [SerializeField] Button buttonUP;
    [SerializeField] Button buttonDown;
    [Space]
    //pannels to enable when there are no monsters.
    [SerializeField] private GameObject monsterPanel2;


    private PlayerInventory playerInventory;
    private Puse puse;
    public GameObject pcPanel;
    private FirstPersonController player;
    private RandomMonsterPicker randomMonsterPicker;

    private GameObject entryHolder;

    void MakeHolder() {
        if (entryHolder != null) {
            Destroy(entryHolder);
        }

        entryHolder = new GameObject("Entry Holder");
        entryHolder.transform.parent = holder.transform;
        VerticalLayoutGroup group = entryHolder.AddComponent<VerticalLayoutGroup>();
        group.childForceExpandHeight = false;
        group.childControlHeight = false;
        group.childControlWidth = false;

        holder.GetComponent<ScrollRect>().content = entryHolder.GetComponent<RectTransform>();
    }

    private void Start() {
        puse = GameObject.FindGameObjectWithTag("PousePannel").GetComponent<Puse>();
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
        player = GameObject.Find("Player").GetComponent<FirstPersonController>();

        pcPanel.SetActive(false);
        monsterPanel2.SetActive(false);
    }
    
    public void HidePcUi() {
        pcPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        player.enabled = true;
        Time.timeScale = 1;
    }

    public void ShowUI() {
        pcPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.enabled = false;
        Time.timeScale = 0;
        SetPlayerMonstersPC();
        LoadPCElements();
    }


    private void SetPlayerMonstersPC() {

        // set player mosnters pc
        Monster primaryMonster = playerInventory.GetMonster().GetComponent<Monster>();

        playerMonstername1.text = primaryMonster.monsterName;
        playerMonsterType1.text = primaryMonster.element.ToString();
        playerMonsterSprite1.sprite = primaryMonster.sprites[0];
        playerMonsterLVL1.text = $"LVL {primaryMonster.monsterLevel}";

        // sond mosnter on schhreen 
        if (playerInventory.GetMonster(true)) {
            monsterPanel2.SetActive(true);
            Monster secondaryMonster = playerInventory.GetMonster(true).GetComponent<Monster>();
            playerMonstername2.text = secondaryMonster.monsterName;
            playerMonsterType1.text = secondaryMonster.element.ToString();
            playerMonsterSprite2.sprite = secondaryMonster.sprites[0];
            playerMonsterLVL2.text = $"LVL {secondaryMonster.monsterLevel}";

        } else {
            monsterPanel2.SetActive(false);
        }
    }

    void LoadPCElements() {
        // Delete old UI elements
        MakeHolder();

        int i = 0;
        foreach(PlayerInventory.PcEntry entry in playerInventory.pcStorage) {
            // Get prefab from name and load relevant data
            GameObject monster = Instantiate(randomMonsterPicker.GetMonsterPrefabByName(entry.name));
            Sprite front = monster.GetComponent<Monster>().sprites[0];
            Element element = monster.GetComponent<Monster>().element;

            // Create dummy monster to parse the data and get the level
            Monster dummy = monster.GetComponent<Monster>();
            dummy.LoadFromString(entry.saveString);
            int level = dummy.monsterLevel;

            Destroy(monster);

            // Create UI element and assign data
            GameObject template = Instantiate(uiTemplatePrefab, entryHolder.transform);
            PcUIEntry pc = template.GetComponent<PcUIEntry>();
            pc.monsterName.text = entry.name;
            pc.monsterLevel.text = $"Lvl. {level}";
            pc.monsterSprite.sprite = front;
            pc.monsterElement.text = element.ToString();
            pc.index = i;

            ++i;
        }


    }

        //@ TODO Monsters uit opslag haalen

        //@ TODO Pc monster pannels eniblen / diablen like in de SetPlayerMonsterPC fuction

        //@ TODO Loop door de list van opgeslagen mosnsters

        //@ TODO Verplaats de speeler mosnters naar de pc en trug 

}
