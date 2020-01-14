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

    IEnumerator PerformMove(Monster current, Monster target, int index) {
        bool isTargetDead = current.moves[index].PerformMove(current, target);

        playerHUD.SetHP(playerMonster.currentHP);
        enemyHUD.SetHP(enemyMonster.currentHP);

        dialoogText.text = $"{current.monsterName} used {current.moves[index].moveName}.";
        if (current.moves[index].type == MoveType.Attack) {
            dialoogText.text += $"\nIt hit for {current.moves[index].val} damage!";
        } else if (current.moves[index].type == MoveType.Recover) {
            dialoogText.text += $"\nIt recovered {current.moves[index].val} life points!";
        }

        yield return new WaitForSeconds(waitTimePlayer);

        if (isTargetDead) {
            if (state == BattleState.PlayerTurn) {
                state = BattleState.Won;
                StartCoroutine(EndBattle());
            } else {
                state = BattleState.Lost;
                StartCoroutine(EndBattle());
            }
        } else {
            if (state == BattleState.PlayerTurn) {
                state = BattleState.EnemyTurn;

                // @TODO: enemy AI

                StartCoroutine(PerformMove(target, current, Random.Range(0, target.moves.Length)));
            } else {
                state = BattleState.PlayerTurn;
                PlayerTurn();
            }
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
            StartCoroutine(PerformMove(playerMonster, enemyMonster, index));
        }

    }
}
