using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpuFanPuzzleManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("CPU temperature at the start of the puzzle")]
    int startTemperature;

    [SerializeField]
    [Tooltip("How often CPU gains 1 �C")]
    int delta;

    [SerializeField]
    [Tooltip("The maximum temperature the CPU can reach")]
    int maxCpuTemp;

    [SerializeField]
    [Tooltip("Thermal paste object")]
    private GameObject thermalPasteReference;

    [SerializeField]
    [Tooltip("4 possible spawn locations for thermal paste")]
    private Transform position0, position1, position2, position3;

    private GameObject spawnedThermalPaste;
    private int randomPosition;
    private bool puzzleActive = false;
    private GameObject player;
    private int currentTemperature;

    // Start is called before the first frame update
    void Start()
    {
        this.currentTemperature = startTemperature;
        SetupPuzzle();

    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleActive)
        {
            if (currentTemperature == maxCpuTemp)
            {
                currentTemperature = -1;
                FailPuzzle();
            }
        }
    }

    IEnumerator CountdownOnWatch()
    {
        PromptScript.instance.updatePrompt("Find the thermal paste hidden inside the level before the CPU temperature reaches 100!\n Once you find the thermal paste, splash it on the CPU", 6);
        while (currentTemperature < maxCpuTemp)
        {
            if (currentTemperature < -2)
            {
                break;
            }
            else if (currentTemperature == startTemperature)
            {
                yield return new WaitForSeconds(5);
                currentTemperature += 5;
                PromptScript.instance.updatePrompt("Current CPU temperature is " + currentTemperature.ToString() + " �C", 1);
            }
            else if (currentTemperature > 0 && currentTemperature < 100)
            {
                yield return new WaitForSeconds(delta);
                currentTemperature++;
                PromptScript.instance.updatePrompt("Current CPU temperature is " + currentTemperature.ToString() + " �C", 1);
            }
        }
    }


    private void SetupPuzzle()
    {
        puzzleActive = true;
        this.currentTemperature = startTemperature;
        StartCoroutine(CountdownOnWatch());
        //Choose a random location to spawn thermal paste
        randomPosition = UnityEngine.Random.Range(0, 4);
        spawnedThermalPaste = Instantiate(thermalPasteReference);
        if (randomPosition == 0)
        {
            spawnedThermalPaste.transform.position = position0.position;
        }
        else if (randomPosition == 1)
        {
            spawnedThermalPaste.transform.position = position1.position;
        }
        else if (randomPosition == 2)
        {
            spawnedThermalPaste.transform.position = position2.position;
        }
        else
        {
            spawnedThermalPaste.transform.position = position3.position;
        }
    }

    private void FailPuzzle()
    {
        PromptScript.instance.updatePrompt("Don't let the CPU reach 100 �C!", 3);
        GameObject champagneInstance = GameObject.Find("Champagne(Clone)");
        Destroy(champagneInstance);
        Invoke("SetupPuzzle", 5);
    }

    public void PuzzleCleared()
    {
        puzzleActive = false;
        PromptScript.instance.updatePrompt("Congratulations! You have beaten the puzzle!", 3);
        // player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 5.142f, 0), Quaternion.identity);
        currentTemperature = -10;

        TransitionManager.completePuzzle();
    }
}
