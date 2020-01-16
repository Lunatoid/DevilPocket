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
    Image textboxBg;

    [SerializeField]
    TextMeshProUGUI choiceBox;

    [SerializeField]
    Image choiceBg;

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

        // textboxBg.color = Color.clear;
        // choiceBg.color = Color.clear;

        Destroy(gameObject);
    }
}
