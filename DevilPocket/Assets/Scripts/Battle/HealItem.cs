using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour {
    public enum HealType {
        paracetamol,
        ibuprofen,
        morphine
    }

    public void ItemHeal(HealType healType, PlayerInventory playerInventory, Monster monster) {
        switch (healType) {
            case HealType.paracetamol:
                if (playerInventory.paracetamolAmount > 0) {
                    monster.currentHP = monster.currentHP + (monster.maxHP / 3);
                    playerInventory.paracetamolAmount--;
                }
                break;

            case HealType.ibuprofen:
                if (playerInventory.ibuprofenAmount > 0) {
                    monster.currentHP = monster.currentHP + (monster.maxHP / 2);
                    playerInventory.ibuprofenAmount--;
                }
                break;

            case HealType.morphine:
                if (playerInventory.morphineAmount > 0) {
                    monster.currentHP = monster.maxHP;
                    playerInventory.morphineAmount--;
                }
                break;
        }
    }





}
