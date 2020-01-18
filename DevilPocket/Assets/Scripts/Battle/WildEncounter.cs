using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Animations;

public class WildEncounter : MonoBehaviour {
    [SerializeField]
    GameObject encouterdMonster;

    [SerializeField]
    TextMeshPro monsterNameText;

    [SerializeField]
    float monsterTriggerDistence = 20.0f;

    [SerializeField]
    float monsterSpeed = 3.5f;

    public Animator transitson;

    GameObject randomMonster;

    GameObject player;
    PlayerInventory playerInventory;
    RandomMonsterPicker randomMonsterPicker;

    Vector3 destination;
    NavMeshAgent agent;
    Transform target;

    Animator monsterAnimator;

    private void Start() {

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        target = player.transform;

        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();

        // Cache agent component and destination
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;

        // sets speed for the animator
        monsterSpeed = agent.speed;

        // Setting the speed var in the blend tree to the speed value of the agent
        monsterAnimator = GetComponent<Animator>();

        // Get a random monster
        randomMonster = randomMonsterPicker.GetRandomMonsterPrefab();
        monsterNameText.text = randomMonster.GetComponent<Monster>().monsterName;
    }

    /// <summary>
    /// the lading of the battel sene when the palyer is in conctact with the ennemy
    /// </summary>
    /// <param name="wildMonsterTrigger"></param>
    private void OnTriggerEnter(Collider wildMonsterTrigger) {
        
        if (wildMonsterTrigger.gameObject.tag == "Player" ) {

            StartCoroutine(LoadBattleScene());
            Debug.Log("loading");
        }
    }

    public IEnumerator LoadBattleScene() {

        //play animation
        transitson.SetTrigger("start");

        // wait for seconds
        yield return new WaitForSeconds(1);

        // load scene 
        playerInventory.enemyMonsters[0] = randomMonster;
        SceneManager.LoadScene("BattleScene");
        Destroy(gameObject);
    }



    private void Update() {
        monsterNameText.transform.LookAt(player.transform);

        // speed is declard within the blendtree 
        monsterAnimator.SetFloat("speed", monsterSpeed);

        // @TODO monster keeps folowing the player after the player wakls out of 
        // the " monsterTriggerDistence ".
        if (Vector3.Distance(destination, target.position)< monsterTriggerDistence) {

            monsterSpeed = agent.speed;
            gameObject.GetComponent<NavMeshAgent>().enabled = true;

            destination = target.position;
            agent.destination = destination;
        }
        else {
            monsterSpeed = 0;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }

        

    }

}
