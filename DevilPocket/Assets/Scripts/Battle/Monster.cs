using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { Normal, Poison, Metal, Fighting }

public class Monster : MonoBehaviour {

    public bool ownedByPlayer = false;

    public string monsterName;
    public int monsterLevel;

    public int damage;

    public int healAmount = 0;
    public int maxHP;
    public int currentHP;

    public Element element;

    public Move[] moves = new Move[3];

    [SerializeField, Header("Element 0 - Front, Element 1 - Back")]
    Sprite[] sprites = new Sprite[2];

    public void SetSprite() {
        int index = (ownedByPlayer) ? 1 : 0;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[index];
    }

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
