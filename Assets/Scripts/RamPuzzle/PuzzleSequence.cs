using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSequence : MonoBehaviour
{
    void Awake()
    {
        GameObject.Find("Timer").GetComponent<Timer>().enabled = false;
        
    }
    void Start()
    {
        PromptScript.instance.updatePrompt("Place the boxes in the correct order. Watch out, each time you get a different order! You have two and a half minutes", 5f);
        StartCoroutine(timerStart());
    }

    void Update()
    {
        
    }

    IEnumerator timerStart(){
        yield return new WaitForSeconds(5f);
        GameObject.Find("Timer").GetComponent<Timer>().enabled = true;
    }
}
