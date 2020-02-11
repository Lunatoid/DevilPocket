using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour {

    PlayerInventory playerInventory = null;

    public float waitTimeEnemy = 1f;
    public float waitTimeEnd = 2f;
    public float waitTimePlayer = 3f;

    [Space]
    public Animator transitson;

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

    [SerializeField]
    AudioClip attackSfx;

    [SerializeField]
    AudioClip healSfx;

    [SerializeField]
    AudioClip levelUpSfx;

    [SerializeField]
    AudioClip runSfx;


    [SerializeField]
    AudioClip wildMuzik;

    [SerializeField]
    AudioClip bossMuzik;

    [SerializeField]
    AudioClip godMuziek;

    AudioSource audioSource;

    AudioSource muziek;

    [Space]
    public BattleState state;

    bool canInteract = true;

    [Space, SerializeField, Header("Random.Range(base+minMod*level, base+maxMod*level)"), Header("Money from battles is calculated like this:\n")]
    float baseMoney = 10.0f;

    [SerializeField]
    float minMoneyMod = 1.0f;
    [SerializeField]
    float maxMoneyMod = 3.5f;

    [Space, SerializeField, Header("Random.Range(base+minMod*level, base+maxMod*level)"), Header("EXP from battles is calculated like this:\n")]
    float baseExp = 50.0f;

    [SerializeField]
    float minExpMod = 1.0f;
    [SerializeField]
    float maxExpMod = 5.0f;

    public GameObject wildBattle;
    public GameObject reactorBattle;

    [SerializeField, Space, Header("Buttons to be disabled if you're in a boss battle")]
    Button[] buttonsToDisable;

    [Space]
    public GameObject hudHolder;

    // Start is called before the first frame update
    void Start() {

        muziek = GameObject.FindGameObjectWithTag("muziek").GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();

        if (!playerInventory) {
            playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        }

        //hudHolder.SetActive(true);

        if (playerInventory.currentBossBattle != null) {
            if (playerInventory.currentBossBattle != Element.Normal) {
                wildBattle.SetActive(false);
                reactorBattle.SetActive(true);
                muziek.clip = bossMuzik;
                muziek.Play();
            } else if (playerInventory.currentBossBattle == Element.Normal) {
                wildBattle.SetActive(false);
                reactorBattle.SetActive(true);
                muziek.clip = godMuziek;
                muziek.Play();
            }
        } else {
            reactorBattle.SetActive(false);
            wildBattle.SetActive(true);
            muziek.clip = wildMuzik;
            muziek.Play();
        }

        state = BattleState.Start;
        StartCoroutine(SetupBattle());

        transitson.SetTrigger("init");
    }

    /// <summary>
    /// Gets called when the player has no valid monsters.
    /// Shows dialog and then switches to the main scene.
    /// </summary>
    IEnumerator EscapeNoValidMonsters() {
        hudHolder.SetActive(false);
        dialoogText.text = $"You enter the battle but none of your monsters are able to fight!";
        yield return new WaitForSeconds(3);
        dialoogText.text = $"You run away as fast as you can!";
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(LoadPreviousScene());
        //SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// Initializes the HUD and other elements.
    /// </summary>
    IEnumerator SetupBattle() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hudHolder.SetActive(false);

        GameObject enemyGo = Instantiate(playerInventory.enemyMonster, enemyBattleStation);
        enemyMonster = enemyGo.GetComponent<Monster>();
        enemyMonster.LevelTo(playerInventory.enemyMonsterLevel);

        LoadEnemyMonsterHud();

        // Find a suitable monster
        GameObject playerGo = playerInventory.GetMonster();

        if (playerGo.GetComponent<Monster>().currentHP <= 0) {
            playerGo = playerInventory.GetMonster(true);

            if (playerGo) {
                if (playerGo.GetComponent<Monster>().currentHP <= 0) {
                    // Both of our monsters have no HP, retreat!
                    StartCoroutine(EscapeNoValidMonsters());
                    yield break;
                } else {
                    // Our second monster is healthy
                    playerInventory.SwitchMonsters();
                }
            } else {
                // GameObject is null, we don't have a second monster
                StartCoroutine(EscapeNoValidMonsters());
                yield break;
            }
        }
        LoadPlayerMonsterHud();

        dialoogText.text = "A wild " + enemyMonster.monsterName + " approaches...";

        yield return new WaitForSeconds(3.5f);

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    /// <summary>
    /// Uses the enemy AI to decide a move to choose.
    /// </summary>
    /// <returns>The index of the move the enemy wants to perform.</returns>
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

    /// <summary>
    /// Performs the specified move of the <c>current</c> monster.
    /// Will switch to either the player's turn or the enemy's turn depending on the <c>state</c>.
    /// </summary>
    /// <param name="current">The monster that is attacking.</param>
    /// <param name="target">The monster that is being attacked.</param>
    /// <param name="index">The move that is being used.</param>
    IEnumerator PerformMove(Monster current, Monster target, int index) {
        hudHolder.SetActive(false);
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
            audioSource.clip = attackSfx;
            audioSource.Play();
            StartCoroutine(DamageBlink(target));
        } else if (current.moves[index].type == MoveType.Recover) {
            dialoogText.text += $"\nIt recovered {current.moves[index].GetCalculatedValue(target.element)} life points!";
            audioSource.clip = healSfx;
            audioSource.Play();
            StartCoroutine(FlashColor(current, Color.green));
        }

        yield return new WaitForSeconds(waitTimePlayer);

        bool isCurrentDead = current.currentHP <= 0;

        bool playerWon = (state == BattleState.PlayerTurn && isTargetDead) || (state == BattleState.EnemyTurn && isCurrentDead);
        bool enemyWon = (state == BattleState.EnemyTurn && isTargetDead) || (state == BattleState.PlayerTurn && isCurrentDead);

        if (playerWon) {
            state = BattleState.Won;
            StartCoroutine(EndBattle());
        } else if (enemyWon) {
            // See if we can switch to our second monster
            GameObject otherMonster = playerInventory.GetMonster(true);
            if (otherMonster && otherMonster.GetComponent<Monster>().currentHP > 0) {
                StartCoroutine(PerformSwitch(true, BattleState.PlayerTurn));
            } else {
                state = BattleState.Lost;
                StartCoroutine(EndBattle());
            }
        } else if (state == BattleState.PlayerTurn) {
            state = BattleState.EnemyTurn;

            StartCoroutine(PerformMove(target, current, EnemyChooseMove()));
        } else {
            state = BattleState.PlayerTurn;
            PlayerTurn();
        }
    }

    private void Update() {
        // We can't do this in the Start() because some of the buttons are either disabled or under disabled parents
        if (playerInventory.currentBossBattle != null) {
            foreach (Button button in buttonsToDisable) {
                button.interactable = false;
            }
        }
    }

    /// <summary>
    /// Ends the battle and switches back to the main scene.
    /// Will show the appropiate text based on whether <c>state</c> is equal to <c>Won</c> or <c>Lost</c>.
    /// </summary>
    /// <param name="killedEnemy">Whether or not the player won by killing the enemy. Only applicable if the player won.</param>
    IEnumerator EndBattle(bool killedEnemy = true) {
        hudHolder.SetActive(false);
        playerInventory.wonLastBattle = state == BattleState.Won;
        if (state == BattleState.Won) {
            if (killedEnemy) {
                // Player killed the enemy
                float moneyFloat = Random.Range(baseMoney + (float)(enemyMonster.monsterLevel + 1) * minMoneyMod,
                                                baseMoney + (float)(enemyMonster.monsterLevel + 1) * maxMoneyMod);

                int money = Mathf.RoundToInt(moneyFloat);
                playerInventory.money += money;

                float expFloat = Random.Range(baseExp + (float)(enemyMonster.monsterLevel + 1) * minExpMod,
                                              baseExp + (float)(enemyMonster.monsterLevel + 1) * maxExpMod);

                int oldLevels = playerMonster.monsterLevel;
                bool leveledUp = playerMonster.AddExp(Mathf.RoundToInt(expFloat));
                int levelsGrown = playerMonster.monsterLevel - oldLevels;
                LoadPlayerMonsterHud();

                // Update any quest data
                playerInventory.UpdateCompletion(GoalType.KillMonsters, 1, enemyMonster.monsterName);

                dialoogText.text = "You got " + money + " coins and " + Mathf.RoundToInt(expFloat) + " experience!";
                yield return new WaitForSeconds(waitTimeEnd);

                if (leveledUp) {
                    audioSource.clip = levelUpSfx;
                    audioSource.Play();

                    StartCoroutine(FlashColor(playerMonster, Color.cyan));

                    dialoogText.text = $"Leveled up {levelsGrown} level{((levelsGrown > 1) ? "s" : "")}!";
                    yield return new WaitForSeconds(waitTimeEnd);
                    dialoogText.text = "Damage increased by " + levelsGrown * playerMonster.damageValue.y + "!\n";
                    dialoogText.text += "Healing increased by " + levelsGrown * playerMonster.healValue.y + "!\n";
                    dialoogText.text += "Hit points increased by " + levelsGrown * (playerMonster.damageValue.y + playerMonster.healValue.y) + "!\n";
                    yield return new WaitForSeconds(waitTimeEnd * 2.0f);
                }

                dialoogText.text = "You won the battle against " + enemyMonster.monsterName + "!";
                yield return new WaitForSeconds(waitTimeEnd);
                
            } else {
                // Player caught the enemy

                // Update any quest data
                playerInventory.UpdateCompletion(GoalType.CatchMonsters, 1, enemyMonster.monsterName);

                // Check if we can equip them immediately 
                if (!playerInventory.GetMonster(true)) {
                    playerInventory.LoadMonsterIntoParty(enemyMonster, true);

                    dialoogText.text = $"{enemyMonster.monsterName} is now in your party!";
                    yield return new WaitForSeconds(waitTimeEnd);
                } else {
                    // Send them to the PC
                    playerInventory.AddMonsterToPc(enemyMonster);

                    dialoogText.text = $"{enemyMonster.monsterName} has been sent to your PC!";
                    yield return new WaitForSeconds(waitTimeEnd);
                }

            }
        } else if (state == BattleState.Lost) {
            dialoogText.text = "You were slain by " + enemyMonster.monsterName + "!";
            yield return new WaitForSeconds(waitTimeEnd);
            dialoogText.text = "You return home in shame...";
            yield return new WaitForSeconds(waitTimeEnd);
        }
        ExitScene();
    }

    /// <summary>
    /// Loads the player hud with all the data from the primary monster.
    /// </summary>
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

    /// <summary>
    /// Loads the hud with all the data from <c>enemyMonster</c>.
    /// </summary>
    void LoadEnemyMonsterHud() {
        enemyMonster.SetSprite();
        enemyHUD.SetHUD(enemyMonster);
    }

    IEnumerator DamageBlink(Monster monster) {
        // How many times it will blink
        const int BLINK_TIMES = 3;

        SpriteRenderer sr = monster.GetComponent<SpriteRenderer>();
        for (int i = 0; i < BLINK_TIMES; ++i) {
            sr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FlashColor(Monster monster, Color color) {
        SpriteRenderer sr = monster.GetComponent<SpriteRenderer>();

        float elapsedTime = 0.0f;
        const float TIME_TO_LERP = 0.25f;

        // White -> Green
        while (elapsedTime < TIME_TO_LERP) {
            elapsedTime += Time.deltaTime;
            sr.color = Color.Lerp(sr.color, color, (elapsedTime / TIME_TO_LERP));
            yield return new WaitForEndOfFrame();
        }

        // Wait
        yield return new WaitForSeconds(0.5f);

        // Green -> White
        elapsedTime = 0.0f;
        while (elapsedTime < TIME_TO_LERP) {
            elapsedTime += Time.deltaTime;
            sr.color = Color.Lerp(sr.color, Color.white, (elapsedTime / TIME_TO_LERP));
            yield return new WaitForEndOfFrame();
        }

    }

    /// <summary>
    /// Pulls the current monster out of the battle scene and back into the player inventory.
    /// </summary>
    void ExitScene() {
        playerMonster.transform.parent = playerInventory.transform;
        playerMonster.gameObject.SetActive(false);
        StartCoroutine(LoadPreviousScene());
        //SceneManager.LoadScene("MainScene");
    }

    public IEnumerator LoadPreviousScene() {

        //play animation
        transitson.SetTrigger("start");

        // wait for seconds
        yield return new WaitForSeconds(1.1f);

        // load scene 
        SceneManager.LoadScene(playerInventory.sceneBeforeBattle);
    }

    /// <summary>
    /// Shows dialog and enables player interaction
    /// </summary>
    void PlayerTurn() {
        hudHolder.SetActive(true);
        dialoogText.text = "Choose an action:";
        canInteract = true;
    }

    /// <summary>
    /// Calls <c>DoMove</c> with index <c>0</c>.
    /// </summary>
    public void DoMove0() {
        DoMove(0);
    }

    /// <summary>
    /// Calls <c>DoMove</c> with index <c>1</c>.
    /// </summary>
    public void DoMove1() {
        DoMove(1);
    }

    /// <summary>
    /// Calls <c>DoMove</c> with index <c>2</c>.
    /// </summary>
    public void DoMove2() {
        DoMove(2);
    }

    /// <summary>
    /// Calls <c>PerformMove</c> with the specified index.
    /// </summary>
    /// <param name="index">The index of the move to do.</param>
    void DoMove(int index) {
        hudHolder.SetActive(false);
        if (state != BattleState.PlayerTurn) {
            return;
        } else {
            if (!canInteract) return;
            canInteract = false;
            StartCoroutine(PerformMove(playerMonster, enemyMonster, index));
        }

    }

    /// <summary>
    /// Attempts to run from the monster.
    /// Will either successfully run or it will fail and trigger the enemy turn.
    /// </summary>
    public void AttemptRun() {
        if (state != BattleState.PlayerTurn || !canInteract) return;
        canInteract = false;

        bool strongOrSameStrength = playerMonster.monsterLevel >= enemyMonster.monsterLevel;

        int percentChance = (strongOrSameStrength) ? 75 : 10;
        StartCoroutine(PerformRun(Random.Range(1, 101) <= percentChance));
    }

    /// <summary>
    /// Individual run logic.
    /// </summary>
    /// <param name="success">If <c>true</c> it will run successfully, if <c>false</c> it will fail.</param>
    IEnumerator PerformRun(bool success) {
        hudHolder.SetActive(false);
        if (success) {
            dialoogText.text = "You successfully ran away!";
            audioSource.clip = runSfx;
            audioSource.Play();
            yield return new WaitForSeconds(waitTimeEnd);
            ExitScene();
        } else {
            dialoogText.text = "You failed to run away!";
            yield return new WaitForSeconds(waitTimeEnd);
            state = BattleState.EnemyTurn;
            StartCoroutine(PerformMove(enemyMonster, playerMonster, EnemyChooseMove()));
        }
    }

    /// <summary>
    /// Attempts to perform a switch of the current monster.
    /// Will call <c>PerformSwitch</c> with the success based on whether or not the other monster has any HP.
    /// </summary>
    public void SwitchMonsters() {
        if (state != BattleState.PlayerTurn || !canInteract) return;
        canInteract = false;

        bool success = playerInventory.GetMonster(true) != null &&
                       playerInventory.GetMonster(true).GetComponent<Monster>().currentHP > 0;

        StartCoroutine(PerformSwitch(success, BattleState.EnemyTurn));
    }

    /// <summary>
    /// Performs the switch logic.
    /// </summary>
    /// <param name="success">If <c>true</c> it will switch it, if <c>false</c> it will show dialog but keep the player turn.</param>
    /// <param name="nextTurn">If the switch fails, which turn it should switch to</param>
    IEnumerator PerformSwitch(bool success, BattleState nextTurn) {
        if (success) {
            hudHolder.SetActive(false);
            // Save current monster
            playerMonster.transform.parent = playerInventory.transform;
            playerMonster.gameObject.SetActive(false);

            // Switch them
            playerInventory.SwitchMonsters();

            // Load the new one
            LoadPlayerMonsterHud();

            dialoogText.text = $"Get 'em, {playerMonster.monsterName}!";
            yield return new WaitForSeconds(waitTimeEnd);

            state = nextTurn;
            if (nextTurn == BattleState.PlayerTurn) {
                PlayerTurn();
            } else if (nextTurn == BattleState.EnemyTurn) {
                StartCoroutine(PerformMove(enemyMonster, playerMonster, EnemyChooseMove()));
            }
        } else {
            GameObject otherMonster = playerInventory.GetMonster(true);
            if (otherMonster) {
                string otherMonsterName = playerInventory.GetMonster(true).GetComponent<Monster>().monsterName;
                dialoogText.text = $"Looks like {otherMonsterName} is not in fighting state...";
                yield return new WaitForSeconds(waitTimeEnd);
            } else {
                dialoogText.text = $"You have no other monster to switch to!";
                yield return new WaitForSeconds(waitTimeEnd);
            }
            PlayerTurn();

        }
    }

    /// <summary>
    /// Uses the heal item.
    /// </summary>
    /// <param name="playerMonster">Whether or not to heal the player or the enemy.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public IEnumerator UseHealItem(bool playerMonster, int amount, string message = "") {
        // Set state immediately so the player can't spam actions
        hudHolder.SetActive(false);
        state = (playerMonster) ? BattleState.EnemyTurn : BattleState.PlayerTurn;

        if (playerMonster) {
            playerHUD.UpdateUsesHUD(this.playerMonster);
        }

        Monster current = (playerMonster) ? this.playerMonster : enemyMonster;

        dialoogText.text = $"{message}\nIt recovered {amount} life points!";
        audioSource.clip = healSfx;
        audioSource.Play();

        StartCoroutine(FlashColor(current, Color.green));

        current.Heal(amount);

        playerHUD.SetHP(this.playerMonster.currentHP);
        enemyHUD.SetHP(enemyMonster.currentHP);

        yield return new WaitForSeconds(waitTimePlayer);

        if (state == BattleState.EnemyTurn) {
            StartCoroutine(PerformMove(enemyMonster, this.playerMonster, EnemyChooseMove()));
        } else {
            PlayerTurn();
        }
    }

    public IEnumerator CatchEnemyMonster() {

        hudHolder.SetActive(false);

        // Set state immediately so the player can't spam actions
        state = BattleState.EnemyTurn;

        playerHUD.UpdateUsesHUD(playerMonster);

        dialoogText.text = $"You lay the bait!";

        yield return new WaitForSeconds(waitTimePlayer);

        // First do a level check
        bool strongOrSameStrength = playerMonster.monsterLevel >= enemyMonster.monsterLevel;

        int percentChance = (strongOrSameStrength) ? 75 : 10;
        if (Random.Range(1, 101) <= percentChance) {
            // We passed the first check, now do another one based on health

            // Get the health percentage where 0.0 is full health and 1.0 is no health
            float healthPercent = 1.0f - (float)enemyMonster.maxHP / (float)enemyMonster.currentHP;
            percentChance = 60 - Mathf.RoundToInt(healthPercent) * 50;
            if (Random.Range(1, 101) <= percentChance) {
                dialoogText.text = $"{enemyMonster.monsterName} fell right for your trap!";
                yield return new WaitForSeconds(waitTimePlayer);

                // We got 'em!
                state = BattleState.Won;
                StartCoroutine(EndBattle(false));
                yield break;
            }

        }

        dialoogText.text = $"{enemyMonster.monsterName} sees right through your trap!";
        yield return new WaitForSeconds(waitTimePlayer);

        StartCoroutine(PerformMove(enemyMonster, playerMonster, EnemyChooseMove()));
    }

    public BattleState GetState() {
        return state;
    }
}
