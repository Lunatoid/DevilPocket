﻿using System.Collections;
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
    public float monsterSpeed = 3.5f;

    [SerializeField]
    float chaseModifier = 1.25f;

    [SerializeField, Header("This will be the cap of the animation speed.")]
    float maxAnimationSpeed = 2.5f;

    public int level = 1;

    [Header("mat changing")]
    public GameObject materialHolder;

    public Material godMaterial;
    public Material iceMaterial;
    public Material metalMaterial;
    public Material posionMaterial;



    // This is the random monster that the player will encounter
    // Decided at Start()
    GameObject randomMonster;

    GameObject player;
    PlayerInventory playerInventory;
    RandomMonsterPicker randomMonsterPicker;

    [HideInInspector] public NavMeshAgent agent;

    Animator monsterAnimator;

    NavMeshTriangulation navMeshTriangles;

    Vector3 GetRandomDestination() {
        // Set agent to random path
        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshTriangles.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshTriangles.vertices[navMeshTriangles.indices[t]], navMeshTriangles.vertices[navMeshTriangles.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshTriangles.vertices[navMeshTriangles.indices[t + 2]], Random.value);

        return point;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();

        randomMonsterPicker = GameObject.Find("RandomMonsterPicker").GetComponent<RandomMonsterPicker>();

        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<Animator>();

        // Apply our own speed
        agent.speed = monsterSpeed;

        // Get a random monster
        randomMonster = randomMonsterPicker.GetRandomMonsterPrefab();
        monsterNameText.text = randomMonster.GetComponent<Monster>().monsterName;

        navMeshTriangles = NavMesh.CalculateTriangulation();
        agent.destination = GetRandomDestination();


        if (randomMonster.GetComponent<Monster>().element != Element.Normal) {
            if (randomMonster.GetComponent<Monster>().element == Element.Ice) {
                materialHolder.GetComponent<SkinnedMeshRenderer>().material = iceMaterial;
            }
            if (randomMonster.GetComponent<Monster>().element == Element.Metal) {
                materialHolder.GetComponent<SkinnedMeshRenderer>().material = metalMaterial;
            }
            if (randomMonster.GetComponent<Monster>().element == Element.Poison) {
                materialHolder.GetComponent<SkinnedMeshRenderer>().material = posionMaterial;
            }
        } else {
            materialHolder.GetComponent<SkinnedMeshRenderer>().material = godMaterial;
        }
    }

    /// <summary>
    /// the lading of the battel sene when the palyer is in conctact with the ennemy
    /// </summary>
    /// <param name="wildMonsterTrigger"></param>
    private void OnTriggerEnter(Collider wildMonsterTrigger) {
        if (wildMonsterTrigger.gameObject.tag == "Player") {
            // Debug.Log("molesting mosnter");
            GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LoadTransition>().SlideUpDown();

            // Debug.Log("loading");
            StartCoroutine(LoadBattleScene());
        }
    }

    public IEnumerator LoadBattleScene() {

        // Debug.Log("Your fate is near");

        yield return new WaitForSeconds(1.1f);

        // load scene 
        playerInventory.enemyMonster = randomMonster;
        playerInventory.enemyMonsterLevel = level;
        SceneManager.LoadScene("BattleScene");
        Destroy(gameObject);
    }


    private void Update() {
        monsterNameText.transform.LookAt(player.transform);

        // speed is declard within the blendtree 
        monsterAnimator.SetFloat("speed", Mathf.Min(agent.velocity.magnitude, maxAnimationSpeed));

        // Stop if the distance between us and the player is greater than the trigger distance
        bool chasingPlayer = Vector3.Distance(transform.position, player.transform.position) < monsterTriggerDistence;

        agent.speed = (chasingPlayer) ? monsterSpeed * chaseModifier : monsterSpeed;

        if (chasingPlayer) {
            agent.destination = player.transform.position;
        } else if (Vector3.Distance(transform.position, agent.destination) < 1.0f) {
            // Reached destination
            agent.destination = GetRandomDestination();
            // Debug.Log("Reached destination!");
        }

        if (!agent.hasPath && !agent.pathPending) {
            // New destination
            agent.destination = GetRandomDestination();
            // Debug.Log("Choosing new random destination...");
        }
    }

}
