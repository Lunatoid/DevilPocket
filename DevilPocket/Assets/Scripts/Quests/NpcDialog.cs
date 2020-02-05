using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class NpcDialog : MonoBehaviour {

    // How close the player needs to be for them to be able to interact
    [SerializeField] float interactDisctance = 3.0f;

    GameObject player;
    PlayerDialogHandler playerDialogHandler;

    [SerializeField, Multiline]
    string[] dialog;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        playerDialogHandler = player.GetComponent<PlayerDialogHandler>();
    }

    // Update is called once per frame
    void Update() {
        if (CrossPlatformInputManager.GetButtonDown("Interact") &&
            Vector3.Distance(transform.position, player.transform.position) < interactDisctance) {
            // Add dialog
            playerDialogHandler.PushDialog(gameObject, dialog);

            // Look at player
            // transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, transform.position.z));
        }
    }
}
