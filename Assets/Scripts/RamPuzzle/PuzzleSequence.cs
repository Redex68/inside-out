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

    IEnumerator timerStart()
    {
        yield return new WaitForSeconds(7f);
        GameObject.Find("Timer").GetComponent<Timer>().enabled = true;
    }

    static string[] RamPuzzle =
    {
        "Place the boxes in the correct order.\nThe clock is ticking!\n",
        "Stavi kutije točnim redoslijedom.\nVrijeme teče!\n"
    };

    IEnumerator startPrompt()
    {
        for(int i = 6; i > 0; i--)
        {
            PromptScript.instance.updatePrompt(Localization.Loc.loc(RamPuzzle) + string.Format("{0}", i), 2.0f);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
