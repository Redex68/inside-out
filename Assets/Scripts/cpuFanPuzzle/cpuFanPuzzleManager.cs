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

    static string[] FindThermalPaste =
    {
        "Find the hidden thermal paste and put it on CPU before temperature reaches 100!",
        "Pronađi skrivenu termalnu pastu i stavi ju na procesor prije nego što dosegne 100 stupnjeva!"
    };

    static string[] CurrentCPUTemp =
    {
        "Current CPU temperature is {0} degrees",
        "Trenutna temperatura procesora je {0} stupnjeva"
    };

    IEnumerator CountdownOnWatch()
    {
        PromptScript.instance.updatePrompt(Localization.Loc.loc(FindThermalPaste), 6);
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
                PromptScript.instance.updatePrompt(string.Format(Localization.Loc.loc(CurrentCPUTemp), currentTemperature.ToString()), 1);
            }
            else if (currentTemperature > 0 && currentTemperature < 100)
            {
                yield return new WaitForSeconds(delta);
                currentTemperature++;
                PromptScript.instance.updatePrompt(string.Format(Localization.Loc.loc(CurrentCPUTemp), currentTemperature.ToString()), 1);
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
        spawnedThermalPaste = Instantiate(thermalPasteReference, transform);
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

    static string[] DontLetCPU =
    {
        "Don't let the CPU reach 100 degrees!",
        "Nemoj dopustiti procesoru da dosegne 100 stupnjeva!"
    };

    private void FailPuzzle()
    {
        PromptScript.instance.updatePrompt(Localization.Loc.loc(DontLetCPU), 3);
        GameObject champagneInstance = GameObject.Find("Champagne(Clone)");
        Destroy(champagneInstance);
        Invoke("SetupPuzzle", 5);
    }

    public void PuzzleCleared()
    {
        puzzleActive = false;
        PromptScript.instance.updatePrompt(Localization.Loc.loc(Localization.StoryTxt.Completed), 3);
        // player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 5.142f, 0), Quaternion.identity);
        currentTemperature = -10;

        TransitionManager.completePuzzle();
    }
}
