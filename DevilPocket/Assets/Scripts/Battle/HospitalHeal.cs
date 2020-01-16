using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalHeal : MonoBehaviour {
    void HealMonster(Monster monster) {
        monster.currentHP = monster.maxHP;
        foreach (Move move in monster.moves) {
            move.uses = move.maxUses;
        }
    }

    void HealMonsters(List<string> args) {
        Debug.Log("Healing monsters...");

        PlayerInventory inventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();

        HealMonster(inventory.GetMonster().GetComponent<Monster>());
        HealMonster(inventory.GetMonster(true).GetComponent<Monster>());
    }
}
