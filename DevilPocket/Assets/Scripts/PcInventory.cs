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

    //PC Invenory UI 4x
    [Header("Pc Monster 1")]
    [Header("PC Inventory UI")]
    [SerializeField] private TextMeshProUGUI pCMonstername1;
    [SerializeField] private Image pCMonsterSprite1;
    [SerializeField] private TextMeshProUGUI pCMonsterType1;
    [SerializeField] private TextMeshProUGUI pCMonsterLVL1;
    [SerializeField] public Button equipMonster1;

    [Header("Pc Monster 2")]
    [SerializeField] private TextMeshProUGUI pCMonstername2;
    [SerializeField] private Image pCMonsterSprite2;
    [SerializeField] private TextMeshProUGUI pCMonsterType2;
    [SerializeField] private TextMeshProUGUI pCMonsterLVL2;
    [SerializeField] public Button equipMonster2;

    [Header("Pc Monster 3")]
    [SerializeField] private TextMeshProUGUI pCMonstername3;
    [SerializeField] private Image pCMonsterSprite3;
    [SerializeField] private TextMeshProUGUI pCMonsterType3;
    [SerializeField] private TextMeshProUGUI pCMonsterLVL3;
    [SerializeField] public Button equipMonster3;

    [Header("Pc Monster 4")]
    [SerializeField] private TextMeshProUGUI pCMonstername4;
    [SerializeField] private Image pCMonsterSprite4;
    [SerializeField] private TextMeshProUGUI pCMonsterType4;
    [SerializeField] private TextMeshProUGUI pCMonsterLVL4;
    [SerializeField] public Button equipMonster4;

    [Space]

    //Buttons navigation 2x

    [Header("Navigation Buttons")]
    [SerializeField] Button buttonUP;
    [SerializeField] Button buttonDown;
    [Space]
    //pannels to enable when there are no monsters.
    [SerializeField] private GameObject monsterPanel2;
    [SerializeField] private GameObject pcMonsterPanel1;
    [SerializeField] private GameObject pcMonsterPanel2;
    [SerializeField] private GameObject pcMonsterPanel3;
    [SerializeField] private GameObject pcMonsterPanel4;


    private PlayerInventory playerInventory;
    private Puse puse;
    public GameObject pcPanel;
    private FirstPersonController player;


    private void Start() {
        puse = GameObject.FindGameObjectWithTag("PousePannel").GetComponent<Puse>();
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        player = GameObject.Find("Player").GetComponent<FirstPersonController>();

        pcPanel.SetActive(false);
        monsterPanel2.SetActive(false);
        pcMonsterPanel1.SetActive(false);
        pcMonsterPanel2.SetActive(false);
        pcMonsterPanel3.SetActive(false);
        pcMonsterPanel4.SetActive(false);
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

        //@ TODO Monsters uit opslag haalen

        //@ TODO Pc monster pannels eniblen / diablen like in de SetPlayerMonsterPC fuction

        //@ TODO Loop door de list van opgeslagen mosnsters

        //@ TODO Verplaats de speeler mosnters naar de pc en trug 

}
