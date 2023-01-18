using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalPasteSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject thermalPasteReference;

    [SerializeField]
    private Transform position0, position1, position2, position3;

    private GameObject spawnedThermalPaste;
    private int randomPosition;

    // Start is called before the first frame update
    void Start()
    {
        randomPosition = Random.Range(0, 4);
        spawnedThermalPaste = Instantiate(thermalPasteReference, transform);
        if (randomPosition == 0)
        {
            spawnedThermalPaste.transform.position = position0.position;
        } else if (randomPosition == 1)
        {
            spawnedThermalPaste.transform.position = position1.position;
        } else if (randomPosition == 2)
        {
            spawnedThermalPaste.transform.position = position2.position;
        } else
        {
            spawnedThermalPaste.transform.position = position3.position;
        }

    }
}
