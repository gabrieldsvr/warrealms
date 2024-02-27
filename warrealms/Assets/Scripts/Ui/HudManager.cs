using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject debugHud;
    [SerializeField] private bool debugMode = false;

    [Space(10), Header("Debug Properties")]

    [SerializeField] private TMP_InputField inputInitialTime;
    [SerializeField] private TMP_InputField inputRateSpawn;

    private NPCSpawner spawner;

    private void Start()
    {
        
    }
    public void ActiveHud()
    {
        if (!debugMode)
        {
            hud.SetActive(!hud.activeSelf);
        }
        else
        {
            if (!spawner)
            {
                spawner = GetComponent<NPCSpawner>();
            }

            spawner.spawnInitialTime = int.Parse(inputInitialTime.text);
            spawner.spawnRate = int.Parse(inputRateSpawn.text);
            debugHud.SetActive(!debugHud.activeSelf);

            Debug.Log("");
        }
    }
}
