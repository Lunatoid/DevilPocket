using System.Collections;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour {

    PlayerInventory playerInventory = null;

    public float waitTimeEnemy = 1f;
    public float waitTimeEnd = 2f;
    public float waitTimeLoad = 4f;
    public float waitTimePlayer = 3f;

    [Space]
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

    bool canInteract = true;

    [Space, SerializeField, Header("Random.Range(base+minMod*level, base+maxMod*level)"), Header("Money from battles is calculated like this:\n")]
    float baseMoney = 10.0f;

    [SerializeField]
    float minMoneyMod = 0.1f;
    [SerializeField]
    float maxMoneyMod = 0.3f;

    // Start is called before the first frame update
    void Start() {
        if (!playerInventory) {
            playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        }

        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Setup monsters
        GameObject playerGo = Instantiate(playerInventory.GetMonster(), playerBattleStation);
        playerMonster = playerGo.GetComponent<Monster>();

        GameObject enemyGo = Instantiate(playerInventory.enemyMonsters[0], enemyBattleStation);
        enemyMonster = enemyGo.GetComponent<Monster>();

        dialoogText.text = "A wild " + enemyMonster.monsterName + " approaches...";

        playerHUD.SetHUD(playerMonster);
        enemyHUD.SetHUD(enemyMonster);

        playerMonster.SetSprite();
        enemyMonster.SetSprite();

        yield return new WaitForSeconds(waitTimeLoad);

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    IEnumerator PlayerMove(int index) {
        bool isEnemyDead = playerMonster.moves[index].PerformMove(playerMonster, enemyMonster);

        playerHUD.SetHP(playerMonster.currentHP);
        enemyHUD.SetHP(enemyMonster.currentHP);

        if (playerMonster.moves[index].type == MoveType.Attack) {
            dialoogText.text = "The attack is successful! " + enemyMonster.monsterName + " was hit for " + playerMonster.moves[index].val + " life points!";
        } else if (playerMonster.moves[index].type == MoveType.Recover) {
            dialoogText.text = playerMonster.monsterName + " feels renewed strength!";
        }

        yield return new WaitForSeconds(waitTimePlayer);

        if (isEnemyDead) {
            state = BattleState.Won;
            StartCoroutine(EndBattle());
        } else {
            state = BattleState.EnemyTurn;
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove() {
        dialoogText.text = enemyMonster.monsterName + " attacks!";
        yield return new WaitForSeconds(waitTimeEnemy);

        // @TODO: choose move based on most damage/strong type/low health heal
        bool isDead = playerMonster.TakeDamage(enemyMonster.damage);

        playerHUD.SetHP(playerMonster.currentHP);

        yield return new WaitForSeconds(waitTimeEnemy);

        if (isDead) {
            state = BattleState.Lost;
            StartCoroutine(EndBattle());
        } else {
            state = BattleState.PlayerTurn;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle() {
        if (state == BattleState.Won) {
            float moneyFloat = Random.Range(baseMoney + (float)enemyMonster.monsterLevel * minMoneyMod,
                                            baseMoney + (float)enemyMonster.monsterLevel * maxMoneyMod);

            int money = Mathf.RoundToInt(moneyFloat);
            playerInventory.money += money;

            dialoogText.text = "You won the battle against " + enemyMonster.monsterName + "!";
            yield return new WaitForSeconds(waitTimeEnd);
            dialoogText.text = "You got " + money + " coins!";
            yield return new WaitForSeconds(waitTimeEnd);
        } else if (state == BattleState.Lost) {
            dialoogText.text = "You were slain by " + enemyMonster.monsterName + "!";
            yield return new WaitForSeconds(waitTimeEnd);
            dialoogText.text = "You return home in shame...";
            yield return new WaitForSeconds(waitTimeEnd);
        }

        SceneManager.LoadScene("MainScene");
    }

    void PlayerTurn() {
        dialoogText.text = "Choose an action:";
        canInteract = true;
    }

    public void DoMove0() {
        DoMove(0);
    }

    public void DoMove1() {
        DoMove(1);
    }

    public void DoMove2() {
        DoMove(2);
    }

    void DoMove(int index) {
        if (state != BattleState.PlayerTurn) {
            return;
        } else {
            if (!canInteract) return;
            canInteract = false;
            StartCoroutine(PlayerMove(index));
        }

    }
}
