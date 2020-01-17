using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WildEncounter : MonoBehaviour
{
    [SerializeField]
    GameObject encouterdMonster;

    private void OnTriggerEnter(Collider wildMonsterTrigger) {
        
        if (wildMonsterTrigger.gameObject.tag == "Player" ) {

            Destroy(gameObject);
            SceneManager.LoadScene("BattleScene");
            Debug.Log("loading");
                      
        }

    }

}
