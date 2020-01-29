using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealItem : MonoBehaviour {
    public enum HealType {
        Paracetamol,
        Ibuprofen,
        Morphine
    }

    PlayerInventory playerInventory;
    BattleSystem battleSystem;

    public void Start() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        battleSystem = GetComponent<BattleSystem>();
    }

    public void ParacetamolButtonClik() {
        ItemHeal(HealType.Paracetamol, playerInventory);
    }

    public void IbuprofenButtonClik() {
        ItemHeal(HealType.Ibuprofen, playerInventory);
    }

    public void MorphineButtonClik() {
        ItemHeal(HealType.Morphine, playerInventory);
    }

    public void ItemHeal(HealType healType, PlayerInventory playerInventory) {
        if (battleSystem.GetState() != BattleState.PlayerTurn) return;

        switch (healType) {
            case HealType.Paracetamol:
                if (playerInventory.paracetamolAmount > 0) {
                    playerInventory.paracetamolAmount--;
                    StartCoroutine(battleSystem.UseHealItem(true, 20, "You used Paracetamol!"));
                    Debug.Log("Paracetamol");
                }
                break;

            case HealType.Ibuprofen:
                if (playerInventory.ibuprofenAmount > 0) {
                    playerInventory.ibuprofenAmount--;
                    StartCoroutine(battleSystem.UseHealItem(true, 50, "You used Ibuprofen!"));
                    Debug.Log("ibuprofen");
                }
                break;

            case HealType.Morphine:
                if (playerInventory.morphineAmount > 0) {
                    playerInventory.morphineAmount--;
                    StartCoroutine(battleSystem.UseHealItem(true, 120, "You used Morphine!"));
                    Debug.Log("morphine");
                }
                break;
        }
    }





}
