using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class MainSceneInit : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI textbox;

    [SerializeField]
    GameObject textboxBg;

    [SerializeField]
    TextMeshProUGUI choiceBox;

    [SerializeField]
    GameObject choiceBg;

    [SerializeField]
    GameObject questNotify;

    GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        // Init dialog handler
        PlayerDialogHandler handler = player.GetComponentInChildren<PlayerDialogHandler>();
        handler.textbox = textbox;
        handler.textboxBg = textboxBg;
        handler.choiceBox = choiceBox;
        handler.choiceBg = choiceBg;
        handler.enabled = true;
        handler.Init();

        if (questNotify) {
            GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>().questNotify = questNotify;
        }

        Destroy(gameObject);
    }
}
