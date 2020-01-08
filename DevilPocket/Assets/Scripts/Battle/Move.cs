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

    [Header("This will either be the base attack or base heal")]
    public int val;

    // Start is called before the first frame update
    void Start() {
        uses = maxUses;
    }

    public bool PerformMove(Monster ownMonster, Monster enemyMonster) {
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
