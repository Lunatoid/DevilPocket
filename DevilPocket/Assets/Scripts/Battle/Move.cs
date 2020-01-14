using System.Collections;
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
    private bool initialized = false;

    [Header("This will either be the base attack or base heal")]
    public int val;

    public bool PerformMove(Monster ownMonster, Monster enemyMonster) {
        if (!initialized) {
            // Initialize max uses
            uses = maxUses;
            initialized = true;
        }

        if (uses > 0) {
            --uses;
        } else {
            return false;
        }

        if (type == MoveType.Attack) {
            return enemyMonster.TakeDamage(val);
        } else if (type == MoveType.Recover) {
            ownMonster.Heal(val);
        }

        return false;
    }
}
