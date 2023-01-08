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
        PromptScript.instance.updatePrompt("Stavi kutije na mjesto pripadajućim rasporedom. Pazi, svako ponavljanje ima različiti raspored! Imaš 30 sekundi.");
        StartCoroutine(timerStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timerStart(){
        yield return new WaitForSeconds(1f);
        GameObject.Find("Timer").GetComponent<Timer>().enabled = true;
    }
}
