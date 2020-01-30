using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class WaterSlowDown : MonoBehaviour {

    FirstPersonController firstPersonController;
    GameObject player;

    void Start() {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {

            firstPersonController.m_WalkSpeed = firstPersonController.m_WalkSpeed / 4;
            firstPersonController.m_RunSpeed = firstPersonController.m_RunSpeed / 4;
            Debug.Log("je loop snelhijd is :" + firstPersonController.m_WalkSpeed);
            Debug.Log("je ren snelhijd is :" + firstPersonController.m_WalkSpeed);
        } else { 
            Debug.Log("je bent uit het water");
            firstPersonController.m_WalkSpeed = firstPersonController.m_WalkSpeed * 4;
            firstPersonController.m_RunSpeed = firstPersonController.m_RunSpeed * 4;
        }
    }
}