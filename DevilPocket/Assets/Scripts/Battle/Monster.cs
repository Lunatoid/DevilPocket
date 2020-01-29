using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { Normal, Poison, Metal, Ice }

public class Monster : MonoBehaviour {

    public bool ownedByPlayer = false;

    public string monsterName;
    public int monsterLevel;

    // Flat values gained per level
    [Header("x = base, y = growth")]
    public Vector2Int damageValue;
    public Vector2Int healValue;

    public int maxHP;
    public int currentHP;

    public Element element;

    public Move[] moves = new Move[3];

    private int currentXP = 0;
    const int BASE_XP = 100;
    private int xpUntilLevelUp = BASE_XP;

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
            moves[i].SetBaseVal((moves[i].type == MoveType.Attack) ? damageValue.x : healValue.x);
            moves[i].SetGrowth((moves[i].type == MoveType.Attack) ? damageValue.y : healValue.y);
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

    public bool AddExp(int exp) {
        currentXP += exp;

        if (currentXP >= xpUntilLevelUp) {
            int remainder = currentXP % xpUntilLevelUp;

            ++monsterLevel;

            currentXP = remainder;

            xpUntilLevelUp = Mathf.RoundToInt((float)BASE_XP * Mathf.Pow((float)monsterLevel, 1.8f));

            foreach (Move move in moves) {
                move.LevelUp();
            }

            return true;
        }
        return false;
    }

    public int GetCurrentExp() {
        return currentXP;
    }

    public int GetExpUntilLevelUp() {
        return xpUntilLevelUp;
    }

    public string SaveToString() {
        string saveString = $"{currentHP} {currentXP} {monsterLevel} {ownedByPlayer},";

        // Save each move
        for (int i = 0; i < moves.Length; ++i) {
            saveString += moves[i].SaveToString();
            if (i + 1 < moves.Length) {
                saveString += ',';
            }
        }

        return saveString;
    }

    public void LoadFromString(string saveString) {
        string[] lines = saveString.Split(',');

        Debug.Assert(lines.Length == 4);

        // Load monster data
        string[] values = lines[0].Split(' ');

        Debug.Assert(values.Length == 4);

        currentHP = int.Parse(values[0]);
        currentXP = int.Parse(values[1]);
        int targetLevel = int.Parse(values[2]);

        // Level up to targetLevel
        for (int i = 0; i < targetLevel - 1; ++i) {
            AddExp(xpUntilLevelUp);
        }

        ownedByPlayer = bool.Parse(values[3]);

        // Load move data
        for (int i = 0; i < moves.Length; ++i) {
            moves[i].LoadFromString(lines[i + 1]);
        }
    }
}
