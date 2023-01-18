using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectCollisionBetweenCpuAndThermalPaste : MonoBehaviour
{
    private cpuFanPuzzleManager manager;

    private int counter = 0;

    void Start() {
        manager = FindObjectOfType<cpuFanPuzzleManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "CPU" && counter == 0)
        {
            counter++;
            manager.PuzzleCleared();
        }
    }
}
