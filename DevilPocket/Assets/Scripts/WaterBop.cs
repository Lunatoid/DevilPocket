﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class WaterBop : MonoBehaviour {

    [SerializeField]
    FirstPersonController firstPersonController;
    float forceAmount = 5.0f;

    GameObject player;


    // If we immediately unmute the audio it will still play the footsteps
    // We set a timer to unmute the audio
    const float LEAVE_WATER_UNMUTE_TIME = 0.5f;
    public bool isInWater = false;
    float leaveWaterTimer = 0.0f;

    void Start() {
        player = GameObject.Find("Player");
        firstPersonController = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }

    void Update() {
        if (isInWater) {
            if (leaveWaterTimer > 0.0f) {
                leaveWaterTimer -= Time.deltaTime;
                //Debug.Log(leaveWaterTimer);
            } else {
                Debug.Log("Unmuting sounds");
                player.GetComponent<FirstPersonController>().UnmuteSounds();
                isInWater = false;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            firstPersonController.m_WalkSpeed = firstPersonController.m_WalkSpeed / 3;
            firstPersonController.m_RunSpeed = firstPersonController.m_RunSpeed / 3;
            Debug.Log("Muting sounds");
            other.GetComponent<FirstPersonController>().MuteSounds();
            isInWater = true;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            //other.GetComponent<CharacterController>().Move(Vector3.up * forceAmount * Time.deltaTime);
            leaveWaterTimer = LEAVE_WATER_UNMUTE_TIME;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            firstPersonController.m_WalkSpeed = firstPersonController.m_WalkSpeed * 3;
            firstPersonController.m_RunSpeed = firstPersonController.m_RunSpeed * 3;
        }
    }

}
