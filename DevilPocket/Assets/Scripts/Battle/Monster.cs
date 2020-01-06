using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public string monsterName;
    public int monsterLevel;

    public int damage;

    public int healAmount = 0;
    public int maxHP;
    public int currentHP;
    public enum Elemnent { Poison, Metal, Fichting }

    public bool TakeDamage(int dmg) {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount) {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

}


