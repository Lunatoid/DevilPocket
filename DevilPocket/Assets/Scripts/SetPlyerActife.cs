using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SetPlyerActife : MonoBehaviour {

    [SerializeField]
    Transform spawnlocation;

    GameObject player;

    FirstPersonController firstPersonControler;

    // Start is called before the first frame update
    void Awake() {
        player = GameObject.Find("Player");
        firstPersonControler = player.GetComponent<FirstPersonController>();

        SetplayerActife();

        Debug.Log("de keg van een player word flase gezet");
        firstPersonControler.enabled = false;
        Debug.Log("de speler poziete is " + player.transform.position);
        player.transform.position = spawnlocation.transform.position;

    }

    private void Start() {
        Debug.Log("de keg van een player is weer true");
        StartCoroutine(UnityIsKut());
    }

    IEnumerator UnityIsKut() {

        yield return new WaitForEndOfFrame();
        firstPersonControler.enabled = true;
    }

    void SetplayerActife() {

        Debug.Log("player sould be active");
        player.gameObject.SetActive(enabled);

    }


}
