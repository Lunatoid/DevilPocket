using System.Collections;
using TMPro;
using UnityEngine;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour
{
    public float waitTimeEnemy = 1f;
    public float waitTimeEnd = 2f;
    public float waitTimeLoad = 4f;
    public float waitTimePlayer = 3f;

    [Space]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Monster playerMonster;
    Monster enemyMonster;

    [Space]
    public TextMeshProUGUI dialoogText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Space]
    public BattleState state;


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerMonster = playerGO.GetComponent<Monster>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyMonster = enemyGo.GetComponent<Monster>();

        dialoogText.text = "A wild " + enemyMonster.monsterName + " approaches...";


        playerHUD.SetHUD(playerMonster);
        enemyHUD.SetHUD(enemyMonster);

        yield return new WaitForSeconds(waitTimeLoad);

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyMonster.TakeDamage(playerMonster.damage);

        enemyHUD.SetHP(enemyMonster.currentHP);
        dialoogText.text = "the attack is successful! " + enemyMonster.monsterName + " was hit for " + playerMonster.damage + " life points";

        yield return new WaitForSeconds(waitTimePlayer);

        if (isDead)
        {
            state = BattleState.Won;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.EnemyTurn;
            StartCoroutine(EnemyAttack());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerMonster.Heal(playerMonster.healAmount);

        playerHUD.SetHP(playerMonster.currentHP);
        dialoogText.text = playerMonster.monsterName + " feels renewd strength!";

        yield return new WaitForSeconds(waitTimePlayer);

        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyAttack());
    }


    IEnumerator EnemyAttack()
    {
        dialoogText.text = enemyMonster.monsterName + " attacks!";
        yield return new WaitForSeconds(waitTimeEnemy);

        // hiero nog toevoegen dat de moster van de tegenstander kan helen wan hij laag in hp is;
        bool isDead = playerMonster.TakeDamage(enemyMonster.damage);

        playerHUD.SetHP(playerMonster.currentHP);

        yield return new WaitForSeconds(waitTimeEnemy);

        if (isDead){
            state = BattleState.Lost;
            EndBattle();
        }
        else {
            state = BattleState.PlayerTurn;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.Won)
        {
            dialoogText.text = "You won the battle agenst" + enemyMonster.monsterName + "!";
            yield return new WaitForSeconds(waitTimeEnd);
        }
        else if (state == BattleState.Lost)
        {
            dialoogText.text = "You where slain by " + enemyMonster.monsterName + "!";
            yield return new WaitForSeconds(waitTimeEnd);
            dialoogText.text = "You retun home in shame";
            yield return new WaitForSeconds(waitTimeEnd);
        }
    }

    void PlayerTurn()
    {
        dialoogText.text = "Choose an action:";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PlayerTurn)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerAttack());
        }
    }

    public void OnHealButton()
    {
        if (state != BattleState.PlayerTurn)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerHeal());
        }
    }

}
