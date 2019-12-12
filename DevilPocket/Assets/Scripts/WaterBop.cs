using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBop : MonoBehaviour {

    [SerializeField]
    float forceAmount = 1000000.0f;

    void OnTriggerStay(Collider other) {
        Debug.Log("Collision with " + other.name);

        if (other.tag == "Player") {
            other.GetComponent<CharacterController>().Move(Vector3.up * forceAmount * Time.deltaTime);
        }
    }
}
