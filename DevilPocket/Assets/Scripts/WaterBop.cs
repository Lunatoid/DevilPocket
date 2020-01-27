using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class WaterBop : MonoBehaviour {

    [SerializeField]
    float forceAmount = 10.0f;

    GameObject player;

    // If we immediately unmute the audio it will still play the footsteps
    // We set a timer to unmute the audio
    const float LEAVE_WATER_UNMUTE_TIME = 0.5f;
    bool isInWater = false;
    float leaveWaterTimer = 0.0f;

    void Start() {
        player = GameObject.Find("Player");
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
            Debug.Log("Muting sounds");
            other.GetComponent<FirstPersonController>().MuteSounds();
            isInWater = true;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<CharacterController>().Move(Vector3.up * forceAmount * Time.deltaTime);
            leaveWaterTimer = LEAVE_WATER_UNMUTE_TIME;
        }
    }
}
