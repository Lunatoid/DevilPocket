﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType {
    Attack,
    Recover
}

public class Move : MonoBehaviour {
    public string moveName;
    public MoveType type;
    public Element element;
    public int maxUses;
    public int uses;

    // Example:
    // TYPE_MOD[Element.Poison][Element.Metal] returns 1.2 because it's 1.2 as strong

    public float[][] TYPE_MOD = new float[][] {
        // Normal
        new float[] { 1.0f, 1.0f, 1.0f, 1.0f },
        // Poison
        new float[] { 1.0f, 1.0f, 1.2f, 0.8f },
        // Metal
        new float[] { 1.0f, 0.8f, 1.0f, 1.2f },
        // Ice
        new float[] { 1.0f, 1.2f, 0.8f, 1.0f }
    };

    [Header("This will either be the base attack or base heal")]
    public int val;
    private int monsterBase = 0;
    private int growth = 0;

    public void SetBaseVal(int baseVal) {
        monsterBase = baseVal;
    }

    public void SetGrowth(int growth) {
        this.growth = growth;
    }

    public bool PerformMove(Monster ownMonster, Monster enemyMonster) {
        if (uses > 0) {
            --uses;
        } else {
            return enemyMonster.currentHP <= 0;
        }

        if (type == MoveType.Attack) {
            return enemyMonster.TakeDamage(GetCalculatedValue(enemyMonster.element));
        } else if (type == MoveType.Recover) {
            ownMonster.Heal(GetCalculatedValue(enemyMonster.element));
        }

        return enemyMonster.currentHP <= 0;
    }

    /// <summary>
    /// Returns the <c>val</c> with type info applied.
    /// </summary>
    /// <param name="enemyElement">The element of the enemy monster.</param>
    /// <returns>The <c>val</c> with type info applied.</returns>
    public int GetCalculatedValue(Element enemyElement) {
        return Mathf.RoundToInt((float)val * TYPE_MOD[(int)element][(int)enemyElement]) + monsterBase;
    }

    public void CopyFromMove(Move other) {
        moveName = other.moveName;
        type = other.type;
        element = other.element;
        maxUses = other.maxUses;
        uses = other.uses;
        val = other.val;
    }

    public void LevelUp() {
        val += growth;
    }

    public string SaveToString() {
        return $"{uses}";
    }

    public void LoadFromString(string saveString) {
        uses = int.Parse(saveString);
    }
}
