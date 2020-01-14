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


    /// <summary>
    /// Applies the monster properties to the HUD
    /// </summary>
    /// <param name="monster">The monster.</param>
    public void SetHUD(Monster monster) {
        nameText.text = monster.monsterName;
        levelText.text = "Lvl " + monster.monsterLevel;
        hpSlider.maxValue = monster.maxHP;
        hpSlider.value = monster.currentHP;

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

   
    public void SetHP(int hp) {
        hpSlider.value = hp;
    }
}
