using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDataGiver : MonoBehaviour
{
    PcInventory pcInventory;
    PlayerDialogHandler playerDialogHandler;

    private void Start() {
        playerDialogHandler = GameObject.Find("Player").GetComponent<PlayerDialogHandler>();
        pcInventory = GameObject.FindGameObjectWithTag("PCInventory").GetComponent<PcInventory>();
    }

    public void PcOpenworld() {
        playerDialogHandler.ClearDialog();
        pcInventory.ShowUI();
    }

}
