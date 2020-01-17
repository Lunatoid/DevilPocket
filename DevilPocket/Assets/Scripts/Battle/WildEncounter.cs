using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WildEncounter : MonoBehaviour
{
    [SerializeField]
    GameObject encouterdMonster;

    [SerializeField]
    TextMeshPro monsterNameText;

    GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player"); 
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
    }

}
