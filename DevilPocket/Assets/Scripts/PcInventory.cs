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

    [Space]
    //pannels to enable when there are no monsters.
    [SerializeField] private GameObject monsterPanel2;

    private PlayerDialogHandler playerDialogHandler;
    private PlayerInventory playerInventory;
    private Puse puse;
    public GameObject pcPanel;
    private FirstPersonController player;
    private RandomMonsterPicker randomMonsterPicker;

    private GameObject entryHolder;

    private List<PcUIEntry> uiEntries = new List<PcUIEntry>();

    void MakeHolder() {
        if (entryHolder != null) {
            Destroy(entryHolder);
        }

        entryHolder = new GameObject("Entry Holder");
        entryHolder.transform.SetParent(holder.transform, false);

        // Add RectTransform and stretch it to the parent
        RectTransform entryHolderRect = entryHolder.AddComponent<RectTransform>();
        entryHolderRect.anchorMin = new Vector2(0.0f, 0.0f);
        entryHolderRect.anchorMax = new Vector2(1.0f, 1.0f);
        entryHolderRect.pivot = new Vector2(0.5f, 0.5f);
        entryHolderRect.offsetMin = new Vector2(0.0f, 0.0f);
        entryHolderRect.offsetMax = new Vector2(0.0f, 0.0f);

        // Add VerticalLayoutGroup so our dynamic elements are all neatly ordered
        VerticalLayoutGroup group = entryHolder.AddComponent<VerticalLayoutGroup>();
        group.childForceExpandHeight = false;
        group.childControlHeight = false;
        group.childControlWidth = false;

        // Add the ContentSizeFitter
        ContentSizeFitter fitter = entryHolder.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

        ScrollRect scroll = holder.transform.parent.GetComponent<ScrollRect>();
        scroll.content = entryHolderRect;
    }

    private void Start() {
        puse = GameObject.FindGameObjectWithTag("PousePannel").GetComponent<Puse>();
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
        player = GameObject.Find("Player").GetComponent<FirstPersonController>();
        playerDialogHandler = GameObject.Find("Player").GetComponent<PlayerDialogHandler>();

        pcPanel.SetActive(false);
        monsterPanel2.SetActive(false);
    }
    
    public void HideUI() {
        pcPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        player.enabled = true;
        Time.timeScale = 1;
        playerDialogHandler.enabled = true;
    }

    public void ShowUI() {
        pcPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.enabled = false;
        Time.timeScale = 0;
        playerDialogHandler.enabled = false;
        SetPlayerMonstersPC();
        LoadPCElements();
    }

    public void StoreCarriedMonster(bool secondary = false) {
        Monster monster = playerInventory.GetMonster(secondary).GetComponent<Monster>();
        playerInventory.AddMonsterToPc(monster);

        playerInventory.carriedMonsters[(secondary) ? 1 : 0] = null;

        if (!secondary) {
            playerInventory.SwitchMonsters();
        }

        GenerateEntry(playerInventory.pcStorage.Count - 1);
        SetPlayerMonstersPC();
    }

    public void SetPlayerMonstersPC() {
        storeMonster1.interactable = playerInventory.GetMonster(true);

        // set player mosnters pc
        Monster primaryMonster = playerInventory.GetMonster().GetComponent<Monster>();

        playerMonstername1.text = primaryMonster.monsterName;
        playerMonsterType1.text = primaryMonster.element.ToString();
        playerMonsterSprite1.sprite = primaryMonster.sprites[0];
        playerMonsterLVL1.text = $"Lvl. {primaryMonster.monsterLevel}";
        
        // sond mosnter on schhreen 
        if (playerInventory.GetMonster(true)) {
            monsterPanel2.SetActive(true);
            Monster secondaryMonster = playerInventory.GetMonster(true).GetComponent<Monster>();
            playerMonstername2.text = secondaryMonster.monsterName;
            playerMonsterType2.text = secondaryMonster.element.ToString();
            playerMonsterSprite2.sprite = secondaryMonster.sprites[0];
            playerMonsterLVL2.text = $"Lvl. {secondaryMonster.monsterLevel}";

        } else {
            monsterPanel2.SetActive(false);
        }
    }

    void LoadPCElements() {
        // Delete old UI elements
        MakeHolder();

        for (int i = 0; i < playerInventory.pcStorage.Count; ++i) {
            GenerateEntry(i);
        }
    }

    void GenerateEntry(int pcIndex) {
        PlayerInventory.PcEntry entry = playerInventory.pcStorage[pcIndex];

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
        GameObject template = Instantiate(uiTemplatePrefab);
        template.transform.SetParent(entryHolder.transform, false);

        PcUIEntry pc = template.GetComponent<PcUIEntry>();
        pc.pcInventory = this;
        pc.monsterName.text = entry.name;
        pc.monsterLevel.text = $"Lvl. {level}";
        pc.monsterSprite.sprite = front;
        pc.monsterElement.text = element.ToString();
        pc.index = pcIndex;

        uiEntries.Add(pc);
    }

    public void RemoveIndex(int index) {
        Destroy(uiEntries[index].gameObject);
        uiEntries.RemoveAt(index);
        playerInventory.pcStorage.RemoveAt(index);

        // Update the index to the correct one
        for (int i = index; i < playerInventory.pcStorage.Count; ++i) {
            uiEntries[i].index -= 1;
        }
    }
}
