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

    /// <summary>
    /// Applies the monster properties to the HUD
    /// </summary>
    /// <param name="monster">The monster.</param>
    public void SetHUD(Monster monster) {
        nameText.text = monster.monsterName;
        levelText.text = "Lvl " + monster.monsterLevel;
        hpSlider.maxValue = monster.maxHP;
        hpSlider.value = monster.currentHP;
    }

    public void SetHP(int hp) {
        hpSlider.value = hp;
    }
}
