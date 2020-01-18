using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Animations;

public class WildEncounter : MonoBehaviour {
    [SerializeField]
    TextMeshPro monsterNameText;

    [SerializeField]
    float monsterTriggerDistence = 20.0f;

    [SerializeField]
    float monsterSpeed = 3.5f;

    [SerializeField, Header("This will be the cap of the animation speed.")]
    float maxAnimationSpeed = 2.5f;

    public Animator transition;
    

    // This is the random monster that the player will encounter
    // Decided at Start()
    GameObject randomMonster;

    GameObject player;
    PlayerInventory playerInventory;
    RandomMonsterPicker randomMonsterPicker;

    NavMeshAgent agent;

    Animator monsterAnimator;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();
        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<Animator>();

        transition = GetComponent<Animator>();

        // Apply our own speed
        agent.speed = monsterSpeed;

        // Get a random monster
        randomMonster = randomMonsterPicker.GetRandomMonsterPrefab();
        monsterNameText.text = randomMonster.GetComponent<Monster>().monsterName;

       // transition.SetTrigger("init");

    }

    /// <summary>
    /// the lading of the battel sene when the palyer is in conctact with the ennemy
    /// </summary>
    /// <param name="wildMonsterTrigger"></param>
    private void OnTriggerEnter(Collider wildMonsterTrigger) {
        if (wildMonsterTrigger.gameObject.tag == "Player") {
            transition.SetTrigger("start");
            StartCoroutine(LoadBattleScene());
            Debug.Log("loading");
            //play animation
        }
    }

    public IEnumerator LoadBattleScene() {
        // wait for seconds
        yield return new WaitForSeconds(1.1f);

        // load scene 
        playerInventory.enemyMonsters[0] = randomMonster;
        SceneManager.LoadScene("BattleScene");
        Destroy(gameObject);
    }



    private void Update() {
        monsterNameText.transform.LookAt(player.transform);

        // speed is declard within the blendtree 
        monsterAnimator.SetFloat("speed", Mathf.Min(agent.velocity.magnitude, maxAnimationSpeed));

        // Stop if the distance between us and the player is greater than the trigger distance
        agent.isStopped = Vector3.Distance(transform.position, player.transform.position) > monsterTriggerDistence;
        if (!agent.isStopped) {
            agent.destination = player.transform.position;
        }
    }

}
