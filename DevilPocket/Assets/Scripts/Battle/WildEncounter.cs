using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Animations;

public class WildEncounter : MonoBehaviour
{
    [SerializeField]
    GameObject encouterdMonster;

    [SerializeField]
    TextMeshPro monsterNameText;

    [SerializeField]
    float monsterTriggerDistence = 20.0f;

    [SerializeField]
    float monsterSpeed = 3.5f;

    GameObject player;

    Vector3 destination;
    NavMeshAgent agent;
    Transform target;

    Animator monsterAnimator;

    

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        target = player.transform;

        // Cache agent component and destination
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;

        // sets speed for the animator
        monsterSpeed = agent.speed;

        // Setting the speed var in the blend tree to the speed value of the agent
        monsterAnimator = GetComponent<Animator>();
        
       
    }


    private void OnTriggerEnter(Collider wildMonsterTrigger) {
        
        if (wildMonsterTrigger.gameObject.tag == "Player" ) {

            Destroy(gameObject);
            SceneManager.LoadScene("BattleScene");
            Debug.Log("loading");
        }
    }

    private void Update() {
        monsterNameText.transform.LookAt(player.transform);


        monsterAnimator.SetFloat("speed", monsterSpeed);

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
