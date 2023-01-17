using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectCollisionBetweenCpuAndThermalPaste : MonoBehaviour
{
    [SerializeField]
    private cpuFanPuzzleManager manager;

    private int counter = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Champagne(Clone)" && counter == 0)
        {
            counter++;
            manager.PuzzleCleared();
        }
    }
}
