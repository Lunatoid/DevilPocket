using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class NpcDialog : MonoBehaviour {

    // How close the player needs to be for them to be able to interact
    const float INTERACT_DISTANCE = 3.5f;

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
            Vector3.Distance(transform.position, player.transform.position) < INTERACT_DISTANCE) {
            playerDialogHandler.PushDialog(dialog);
        }
    }
}
