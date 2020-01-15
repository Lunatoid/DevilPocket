using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour {


    [SerializeField]
    TextMeshProUGUI nameText;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    Slider hpSlider;


    [Space(20)]
    [Header("Monster Types")]
    [SerializeField]
    GameObject elementPoison;

    [SerializeField]
    GameObject elementMetal;

    [SerializeField]
    GameObject elementIce;


    [Space(20)]
    [Header("Move Types")]
    [SerializeField]
    GameObject moveElementNormaal;

    [SerializeField]
    GameObject moveElementPoison;

    [SerializeField]
    GameObject moveElementMetal;

    [SerializeField]
    GameObject moveElementIce;

    [Space(20)]
    [Header("Move Types Position")]
    [SerializeField]
    Transform move1Position;

    [SerializeField]
    Transform move2Position;

    [SerializeField]
    Transform move3Position;

    [Space(20)]
    [Header("Move Name")]
    [SerializeField]
    TextMeshProUGUI move1NameText;

    [SerializeField]
    TextMeshProUGUI move2NameText;

    [SerializeField]
    TextMeshProUGUI move3NameText;


    [Space(20)]
    [Header("Move Name")]
    [SerializeField]
    TextMeshProUGUI move1Uses;

    [SerializeField]
    TextMeshProUGUI move2Uses;

    [SerializeField]
    TextMeshProUGUI move3Uses;


    /// <summary>
    /// Applies the monster properties to the HUD
    /// </summary>
    /// <param name="monster">The monster.</param>
    public void SetHUD(Monster monster) {
        nameText.text = monster.monsterName;
        levelText.text = "Lvl " + monster.monsterLevel;
        hpSlider.maxValue = monster.maxHP;
        hpSlider.value = monster.currentHP;

        // fully woriking element loding
        if (monster.element == Element.Poison) {
            Instantiate(elementPoison, transform.parent);
        }
        if (monster.element == Element.Metal) {
            Instantiate(elementMetal, transform.parent);
        }
        if (monster.element == Element.Ice) {
            Instantiate(elementIce, transform.parent);
        }
    }

    public void SetMovesHUD(Monster monster) {
        // names of the moves
        move1NameText.text = monster.moves[0].moveName;
        move2NameText.text = monster.moves[1].moveName;
        move3NameText.text = monster.moves[2].moveName;

        // sets the icon vor the moves micht needt to relocate the locatin of the move buttons

        //for move 1
        if (monster.moves[0].element == Element.Normal) {
            Instantiate(moveElementNormaal, move1Position);
        }
        if (monster.moves[0].element == Element.Poison) {
            Instantiate(moveElementPoison, move1Position);
        }
        if (monster.moves[0].element == Element.Metal) {
            Instantiate(moveElementMetal, move1Position);
        }
        if (monster.moves[0].element == Element.Ice) {
            Instantiate(moveElementIce, move1Position);
        }

        // for move 2
        if (monster.moves[1].element == Element.Normal) {
            Instantiate(moveElementNormaal, move2Position);
        }
        if (monster.moves[1].element == Element.Poison) {
            Instantiate(moveElementPoison, move2Position);
        }
        if (monster.moves[1].element == Element.Metal) {
            Instantiate(moveElementMetal, move2Position);
        }
        if (monster.moves[1].element == Element.Ice) {
            Instantiate(moveElementIce, move2Position);
        }

        //for move 3
        if (monster.moves[2].element == Element.Normal) {
            Instantiate(moveElementNormaal, move3Position);
        }
        if (monster.moves[2].element == Element.Poison) {
            Instantiate(moveElementPoison, move3Position);
        }
        if (monster.moves[2].element == Element.Metal) {
            Instantiate(moveElementMetal, move3Position);
        }
        if (monster.moves[2].element == Element.Ice) {
            Instantiate(moveElementIce, move3Position);
        }
    }

    public void UpdateUsesHUD(Monster monster) {
        // pp use
        move1Uses.text = monster.moves[0].uses + "/" + monster.moves[0].maxUses;
        move2Uses.text = monster.moves[1].uses + "/" + monster.moves[1].maxUses;
        move3Uses.text = monster.moves[2].uses + "/" + monster.moves[2].maxUses;

        // Debug.Log(monster.moves[0].uses + " " + monster.moves[0].initialized);
        // Debug.Log(monster.moves[1].uses + " " + monster.moves[1].initialized);
        // Debug.Log(monster.moves[2].uses + " " + monster.moves[2].initialized);
    }

    //works fine.
    public void SetHP(int hp) {
        hpSlider.value = hp;
    }
}
