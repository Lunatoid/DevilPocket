using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealItem : MonoBehaviour {
    public enum HealType {
        paracetamol,
        ibuprofen,
        morphine
    }

    PlayerInventory playerInventory;

    Monster monster;

    public void ParacetamolButtonClik() {
        ItemHeal(HealType.paracetamol, playerInventory, monster);
    }

    public void IbuprofenButtonClik() {
        ItemHeal(HealType.ibuprofen, playerInventory, monster);
    }

    public void MorphineButtonClik() {
        ItemHeal(HealType.morphine, playerInventory, monster);
    }


    public void ItemHeal(HealType healType, PlayerInventory playerInventory, Monster monster) {
        switch (healType) {
            case HealType.paracetamol:
                if (playerInventory.paracetamolAmount > 0) { // gives here an error
                    monster.currentHP = monster.currentHP + (monster.maxHP / 3);
                    playerInventory.paracetamolAmount--;
                    Debug.Log("Paracetamol");
                }
                break;

            case HealType.ibuprofen:
                if (playerInventory.ibuprofenAmount > 0) { // gives here an error
                    monster.currentHP = monster.currentHP + (monster.maxHP / 2);
                    playerInventory.ibuprofenAmount--;
                    Debug.Log("ibuprofen");
                }
                break;

            case HealType.morphine:
                if (playerInventory.morphineAmount > 0) { // gives here an error
                    monster.currentHP = monster.maxHP;
                    playerInventory.morphineAmount--;
                    Debug.Log("morphine");
                }
                break;
        }
    }





}
