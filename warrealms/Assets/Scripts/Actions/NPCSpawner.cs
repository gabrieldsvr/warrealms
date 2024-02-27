using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabNPC;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool automaticSpawn;

    public float spawnInitialTime;
    public float spawnRate;

    private void Start()
    {
        if(automaticSpawn)
        {
            InvokeRepeating("SpawnNPC", spawnInitialTime, spawnRate);
        }
    }
    public void SpawnNPC()
    {
        GameObject newNPC = Instantiate(prefabNPC, spawnPoint);
    }
}
