using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { Normal, Poison, Metal, Ice }

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

    private void Start() {
        // UNITY FOR SOME REASON DOESN'T COPY THE PREFAB SO WHEN YOU
        // DO A MOVE IT WILL EDIT THE PREFAB AT RUNTIME.
        //
        // SO WHAT IS THE SOLUTION?
        // MAKING NEW COMPONENTS AND THEN COPYING EACH PARAMETER!!!!!!!
        for (int i = 0; i < moves.Length; ++i) {
            Move newMove = gameObject.AddComponent<Move>();
            newMove.CopyFromMove(moves[i]);
            moves[i] = newMove;
        }
    }

    private void OnDestroy() {
        // Unity edits the prefab for some reason????
        foreach (Move move in moves) {
            move.uses = move.maxUses;
        }
    }

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
