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
    public void SetHUD(Move move) {

        // names of the moves
        move1NameText.text = move.moveName;
        move2NameText.text = move.moveName;
        move3NameText.text = move.moveName;

        // pp use
        move1Uses.text = move.uses + "/" + move.maxUses;
        move2Uses.text = move.uses + "/" + move.maxUses;
        move3Uses.text = move.uses + "/" + move.maxUses;

        // sets the icon vor the moves micht needt to relocate the locatin of the move buttons
        if (move.element == Element.Normal) {
            Instantiate(moveElementNormaal, transform.parent);
        }
        if (move.element == Element.Poison) {
            Instantiate(moveElementPoison, transform.parent);
        }
        if (move.element == Element.Metal) {
            Instantiate(moveElementMetal, transform.parent);
        }
        if (move.element == Element.Ice) {
            Instantiate(moveElementIce, transform.parent);
        }

    }
        public void SetHP(int hp) {
        hpSlider.value = hp;
    }
}
