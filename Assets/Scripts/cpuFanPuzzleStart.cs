using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cpuFanPuzzleStart : MonoBehaviour
{
    public GameObject cpuFan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cpuCooler")
        {
            SceneManager.LoadScene("cpuFanPuzzle");
        }
    }
}
