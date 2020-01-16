using System.Collections;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour {

    PlayerInventory playerInventory = null;

    public float waitTimeEnemy = 1f;
    public float waitTimeEnd = 2f;
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
        SetupBattle();
    }

    IEnumerator EscapeNoValidMonsters() {
        dialoogText.text = $"You enter the battle but none of your monsters are able to fight!";
        yield return new WaitForSeconds(waitTimeEnd);
        dialoogText.text = $"You run away as fast as you can!";
        yield return new WaitForSeconds(waitTimeEnd);
        SceneManager.LoadScene("MainScene");
    }

    void SetupBattle() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject enemyGo = Instantiate(playerInventory.enemyMonsters[0], enemyBattleStation);
        enemyMonster = enemyGo.GetComponent<Monster>();

        LoadEnemyMonsterHud();

        // Find a suitable monster
        GameObject playerGo = playerInventory.GetMonster();

        if (playerGo.GetComponent<Monster>().currentHP <= 0) {
            playerGo = playerInventory.GetMonster(true);
            if (playerGo.GetComponent<Monster>().currentHP <= 0) {
                // Both of our monsters have no HP, retreat!
                StartCoroutine(EscapeNoValidMonsters());
                return;
            } else {
                // Our second monster is healthy
                playerInventory.SwitchMonsters();
            }
        }
        LoadPlayerMonsterHud();

        dialoogText.text = "A wild " + enemyMonster.monsterName + " approaches...";

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    int EnemyChooseMove() {
        // Enemy AI
        int moveIndex = 0;
        MoveType targetType;

        float healthPercent = (float)enemyMonster.currentHP / (float)enemyMonster.maxHP;

        if (healthPercent < 0.15f) {
            targetType = MoveType.Recover;
        } else if (healthPercent < 0.5f) {
            // 1 in HEAL_CHANCE times it will heal
            const int HEAL_CHANCE = 3;

            bool shouldHeal = Random.Range(0, HEAL_CHANCE) == 0;

            // Find a move
            targetType = (shouldHeal) ? MoveType.Recover : MoveType.Attack;
        } else {
            targetType = MoveType.Attack;
        }

        Debug.Log("Enemy wants to " + targetType);

        // Every time we pick a random move and it's not a desired move we add 1 to the rejectedMoves
        // If the rejectedMoves reaches the REJECTED_THRESHOLD it will just pick a random move
        int rejectedMoves = 0;
        const int REJECTED_THRESHOLD = 50;

        while (true) {
            int randomIndex = Random.Range(0, enemyMonster.moves.Length);

            if (enemyMonster.moves[randomIndex].type == targetType) {
                // Check if it has any uses left
                if (enemyMonster.moves[randomIndex].uses > 0) {
                    Debug.Log("Enemy chose " + enemyMonster.moves[randomIndex].moveName);
                    moveIndex = randomIndex;
                    break;
                } else {
                    ++rejectedMoves;
                }
            } else {
                ++rejectedMoves;
            }


            if (rejectedMoves >= REJECTED_THRESHOLD) {
                Debug.Log("Rejection threshold reached. Enemy chose " + enemyMonster.moves[randomIndex].moveName);
                moveIndex = randomIndex;
                break;
            }
        }

        return moveIndex;
    }

    IEnumerator PerformMove(Monster current, Monster target, int index) {
        int uses = current.moves[index].uses;
        bool isTargetDead = current.moves[index].PerformMove(current, target);
        playerHUD.UpdateUsesHUD(playerMonster);

        playerHUD.SetHP(playerMonster.currentHP);
        enemyHUD.SetHP(enemyMonster.currentHP);


        dialoogText.text = $"{current.monsterName} used {current.moves[index].moveName}.";
        if (uses == 0) {
            dialoogText.text += "\nBut it was out of uses!";
        } else if (current.moves[index].type == MoveType.Attack) {
            dialoogText.text += $"\nIt hit for {current.moves[index].GetCalculatedValue(target.element)} damage!";
        } else if (current.moves[index].type == MoveType.Recover) {
            dialoogText.text += $"\nIt recovered {current.moves[index].GetCalculatedValue(target.element)} life points!";
        }

        yield return new WaitForSeconds(waitTimePlayer);

        if (isTargetDead) {
            if (state == BattleState.PlayerTurn) {
                state = BattleState.Won;
                StartCoroutine(EndBattle());
            } else {
                // See if we can switch to our second monster
                if (playerInventory.GetMonster(true).GetComponent<Monster>().currentHP > 0) {
                    StartCoroutine(PerformSwitch(true));
                } else {
                    state = BattleState.Lost;
                    StartCoroutine(EndBattle());
                }
            }
        } else {
            if (state == BattleState.PlayerTurn) {
                state = BattleState.EnemyTurn;

                StartCoroutine(PerformMove(target, current, EnemyChooseMove()));
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
        ExitScene();
    }

    void LoadPlayerMonsterHud() {
        GameObject playerGo = playerInventory.GetMonster();
        playerGo.SetActive(true);
        playerGo.transform.parent = playerBattleStation;
        playerGo.transform.position = new Vector3(0.0f, 0.0f);
        playerGo.transform.localPosition = new Vector3(0.0f, 0.0f);
        playerMonster = playerGo.GetComponent<Monster>();
        playerHUD.SetHUD(playerMonster);
        playerMonster.SetSprite();
        playerHUD.SetMovesHUD(playerMonster);
        playerHUD.UpdateUsesHUD(playerMonster);
    }

    void LoadEnemyMonsterHud() {
        enemyMonster.SetSprite();
        enemyHUD.SetHUD(enemyMonster);
    }

    void ExitScene() {
        playerMonster.transform.parent = playerInventory.transform;
        playerMonster.gameObject.SetActive(false);
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

    public void AttemptRun() {
        if (state != BattleState.PlayerTurn || !canInteract) return;
        canInteract = false;

        // @TODO: better run algorithm
        bool strongOrSameStrength = playerMonster.monsterLevel >= enemyMonster.monsterLevel;

        int percentChance = (strongOrSameStrength) ? 75 : 10;
        StartCoroutine(PerformRun(Random.Range(1, 101) <= percentChance));
    }

    IEnumerator PerformRun(bool success) {
        if (success) {
            dialoogText.text = "You successfully ran away!";
            yield return new WaitForSeconds(waitTimeEnd);
            ExitScene();
        } else {
            dialoogText.text = "You failed to run away!";
            yield return new WaitForSeconds(waitTimeEnd);
            state = BattleState.EnemyTurn;
            StartCoroutine(PerformMove(enemyMonster, playerMonster, EnemyChooseMove()));
        }
    }

    public void SwitchMonsters() {
        if (state != BattleState.PlayerTurn || !canInteract) return;
        canInteract = false;

        StartCoroutine(PerformSwitch(playerInventory.GetMonster(true).GetComponent<Monster>().currentHP > 0));
    }

    IEnumerator PerformSwitch(bool success) {
        if (success) {
            // Save current monster
            playerMonster.transform.parent = playerInventory.transform;
            playerMonster.gameObject.SetActive(false);

            // Switch them
            playerInventory.SwitchMonsters();

            // Load the new one
            LoadPlayerMonsterHud();

            dialoogText.text = $"Get 'em, {playerMonster.monsterName}!";
            yield return new WaitForSeconds(waitTimeEnd);

            state = BattleState.EnemyTurn;
            StartCoroutine(PerformMove(enemyMonster, playerMonster, EnemyChooseMove()));
        } else {
            string otherMonsterName = playerInventory.GetMonster(true).GetComponent<Monster>().monsterName;
            dialoogText.text = $"Looks like {otherMonsterName} is not in fighting state...";
            yield return new WaitForSeconds(waitTimeEnd);

            PlayerTurn();
        }
    }
}
