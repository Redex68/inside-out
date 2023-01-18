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
        StartCoroutine(startPrompt());
        StartCoroutine(timerStart());
    }

    void Update()
    {
        
    }

    IEnumerator timerStart(){
        yield return new WaitForSeconds(7f);
        GameObject.Find("Timer").GetComponent<Timer>().enabled = true;
    }

    IEnumerator startPrompt(){
        for(int i = 7; i >= 0; i--){
            PromptScript.instance.updatePrompt("Place the boxes in the correct order. \nWatch out, each time you get a different order! \nYou have a minute and a half.\n" + string.Format("{0:00} : {1:00}", 0,  Mathf.FloorToInt(i % 60)));
            yield return new WaitForSeconds(1f);
        }
    }
}
