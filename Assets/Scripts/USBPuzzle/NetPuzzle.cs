using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lateStart());
    }

    IEnumerator lateStart()
    {
        yield return new WaitForSeconds(1.0f);

        onComplete();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static string[] Connected =
    {
        "Connected to a network!",
        "Povezano na mrežu!"
    };

    void onComplete()
    {
        Router.Instance.completed = true;
        PromptScript.instance.updatePrompt(Localization.Loc.loc(Localization.StoryTxt.Completed), 3.0f);
        TransitionManager.completePuzzle();
    }
}
