using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDataGiver : MonoBehaviour
{
    PcInventory pcInventory;

    private void Start() {
        pcInventory = GameObject.FindGameObjectWithTag("PCInventory").GetComponent<PcInventory>();
    }

    public void PcOpenworld() {
        pcInventory.ShowUI();
    }

}
