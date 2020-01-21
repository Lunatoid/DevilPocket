using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour {

    class EncounterEntry {
        public GameObject instance = null;
        public System.DateTime instancedTime = System.DateTime.Now;
    }

    List<EncounterEntry> spawnedEncounters;

    [SerializeField]
    GameObject encounterPrefab;

    [SerializeField]
    float despawnTimeInSeconds = 300.0f;

    [SerializeField, Header("Monster's despawn time is despawnTimeInSeconds (+-) 1.0..despawnMargin")]
    float despawnMargin = 60.0f;

    [SerializeField, Header("Amount of encounters that should be spawned at any given time")]
    int encounterCap;

    [SerializeField, Header("How many seconds to wait until spawning the encounters")]
    float timeUntilSpawning;

    [SerializeField, Header("The interval in checking if encounters should despawn and spawn new ones")]
    float despawnCheckInterval;

    // Start is called before the first frame update
    void Awake() {
        DontDestroyOnLoad(gameObject);
        spawnedEncounters = new List<EncounterEntry>(encounterCap);
        
        for (int i = 0; i < encounterCap; ++i) {
            EncounterEntry entry = new EncounterEntry();
            spawnedEncounters.Add(entry);
        }

        SceneManager.activeSceneChanged += ToggleEncounters;
    }

    private void ToggleEncounters(Scene arg0, Scene arg1) {
        bool active = arg1.name == "MainScene";

        foreach (EncounterEntry entry in spawnedEncounters) {
            if (entry.instance) {
                entry.instance.SetActive(active);
            }
        }

        gameObject.SetActive(active);

        if (active) {
            InvokeRepeating("SweepAndSpawn", timeUntilSpawning, despawnCheckInterval);
        } else {
            CancelInvoke("SweepAndSpawn");
        }
    }

    void SweepAndSpawn() {
        Debug.Log("Starting SweepAndSpawn...");

        // First sweep any instances that should despawn
        for (int i = 0; i < spawnedEncounters.Count; ++i) {
            if (System.DateTime.Now.Subtract(spawnedEncounters[i].instancedTime).TotalSeconds >= despawnTimeInSeconds) {
                Destroy(spawnedEncounters[i].instance);
                spawnedEncounters[i].instance = null;
                Debug.Log("Despawning encounter");
            }
        }

        // Spawn any new ones
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("EncounterSpawner");

        for (int i = 0; i < spawnedEncounters.Count; ++i) {
            if (spawnedEncounters[i].instance == null) {

                GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Vector3 spawnPos = spawnPoint.transform.position;

                // Raycast down to find the ground
                RaycastHit hitInfo;
                int mask = 1 << LayerMask.NameToLayer("terain");
                if (Physics.Raycast(spawnPoint.transform.position, Vector3.down, out hitInfo, 100000000.0f, mask)) {
                    spawnPos = hitInfo.point;
                } else {
                    // Abort
                    continue;
                }

                spawnedEncounters[i].instance = Instantiate(encounterPrefab, spawnPos, Quaternion.identity, transform);

                spawnedEncounters[i].instancedTime = System.DateTime.Now;
                bool plusMargin = Random.Range(0.0f, 1.0f) > 0.5f;

                float margin = Random.Range(0.0f, despawnMargin);
                if (!plusMargin) {
                    margin = -margin;
                }

                spawnedEncounters[i].instancedTime.AddSeconds(margin);

                Debug.Log("Spawning encounter at " + spawnPoint.name + "with a despawn time of " + (despawnTimeInSeconds + margin));

            }
        }
    }
}
