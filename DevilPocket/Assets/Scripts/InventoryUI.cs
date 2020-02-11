using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class InventoryUI : MonoBehaviour {

    [Header("Monny")]
    [SerializeField] private TextMeshProUGUI coins;

    [Header("Moster1 Info")]
    [SerializeField] private TextMeshProUGUI monstername1;
    //[SerializeField] private Image mosnterImage1;
    [SerializeField] public Image monsterSprite1;
    [SerializeField] private TextMeshProUGUI monsterType1;
    [SerializeField] private TextMeshProUGUI monsterHP1;
    [SerializeField] private TextMeshProUGUI monsterLVL1;

    [Header("Moster2 Info")]
    [SerializeField] private TextMeshProUGUI monstername2;
    //[SerializeField] private Image mosnterImage2;
    [SerializeField] public Image monsterSprite2;
    [SerializeField] private TextMeshProUGUI monsterType2;
    [SerializeField] private TextMeshProUGUI monsterHP2;
    [SerializeField] private TextMeshProUGUI monsterLVL2;

    [Space(2)]
    [Header("Items")]
    [SerializeField] private TextMeshProUGUI baitAmount;
    [SerializeField] private TextMeshProUGUI paracetamolAmount;
    [SerializeField] private TextMeshProUGUI ibuprofenAmount;
    [SerializeField] private TextMeshProUGUI morphineAmount;

    [SerializeField] private GameObject monster2Pannel;

    [SerializeField] private TextMeshProUGUI battlePassText;

    private PlayerInventory playerInventory;
    private Puse puse;

    [HideInInspector]public bool hasShownPP = false;

    private void Start() {
        puse = GameObject.FindGameObjectWithTag("PousePannel").GetComponent<Puse>();
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
    }

    private void Update() {
        if (puse.ispouse && !hasShownPP) {
            hasShownPP = true;
            SetInventoty();
        }
        else {
            hasShownPP = false;
        }
        
    }
    private void SetInventoty() {
        // monny on schreen
        coins.text = $"Coins {playerInventory.money.ToString()}";

        // battle pass text
        if (playerInventory.hasBattlepass == false) {
            battlePassText.text = "No batlepass yet. \nGo buy it at the shop!";
        } else {
            battlePassText.text = "You got the Battle pass! ";
        }

        //items on schreen
        baitAmount.text = ("Bait " + playerInventory.baitAmount);
        paracetamolAmount.text = ("Paracetamol " + playerInventory.paracetamolAmount);
        ibuprofenAmount.text = ("Ibuprofen " + playerInventory.ibuprofenAmount);
        morphineAmount.text = ("Morphine " + playerInventory.morphineAmount);

        // mosters on schreen 
        Monster primaryMonster = playerInventory.GetMonster().GetComponent<Monster>();

        monstername1.text = primaryMonster.monsterName;
        monsterHP1.text = $"{primaryMonster.currentHP} {'/'} {primaryMonster.maxHP}";
        monsterType1.text = primaryMonster.element.ToString();
        monsterSprite1.sprite = primaryMonster.sprites[0];
        monsterLVL1.text = $"LVL {primaryMonster.monsterLevel}";

        // sond mosnter on schhreen 
        if (playerInventory.GetMonster(true)) {
            monster2Pannel.SetActive(true);
            Monster secondaryMonster = playerInventory.GetMonster(true).GetComponent<Monster>();
            monstername2.text = secondaryMonster.monsterName;
            monsterHP2.text = $"{secondaryMonster.currentHP} {'/'} {secondaryMonster.maxHP}";
            monsterType2.text = secondaryMonster.element.ToString();
            monsterSprite2.sprite = secondaryMonster.sprites[0];
            monsterLVL2.text = $"LVL {secondaryMonster.monsterLevel}";

        }else {
            monster2Pannel.SetActive(false);
        }



    }

}
