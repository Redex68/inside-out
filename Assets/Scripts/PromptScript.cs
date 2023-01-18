using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
    <summary>
        A singleton used to control the watch prompt. An instance of the singleton can
        be accessed through the instance parameter and updatePrompt() can be used to change
        the prompt that is displayed.
    </summary>
*/
public class PromptScript: MonoBehaviour
{
    public static PromptScript instance {get; private set;}
    private TMPro.TMP_Text TMPtext;
    private UnityEngine.UI.Image promptBackground;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this;     
        }
    }

    private void Start()
    {
        TMPtext = GameObject.Find("WatchParent/WatchCanvas/PromptText").GetComponent<TMPro.TMP_Text>();
        promptBackground = GameObject.Find("WatchParent/WatchCanvas").GetComponent<UnityEngine.UI.Image>();
        promptBackground.CrossFadeAlpha(0, 0, false);
        
        StartCoroutine(test("Test 1", 0));
        StartCoroutine(test("Jako dugi teaskj lksd jklas djklasd jklasd ljkaskljd assaljkdk ad klčad klččklad sklčas ddklčaklčasd klč asdd klčas asdklč", 2f));
        StartCoroutine(test("asodj aslkd jaksld ja \n asdo aslkd ajkd askd alk d\n asjdaskjd ajsk", 4f));
        StartCoroutine(test("Translacija tvoje stare u tvoju novu", 6f));
        
    }

    IEnumerator test(String text, float time){
        yield return new WaitForSeconds(time);
        updatePrompt(text, 1.0f);
    }

/**
    <summary>
        Fetches the text value of the currently displayed prompt.
    </summary>

    <returns>
        The text value of the currently displayed prompt.
    </returns>
*/
    public string getPrompt(){
        return TMPtext.text;
    }

/**
    <summary>
        Updates the currently displayed prompt with the provided text. The old prompt is
        overriden.
    </summary>
    <param name="text"> The new prompt text. </param>
    <returns> The new text value of the prompt. </returns>
*/
    public void updatePrompt(string text){
        if(text == null) throw new NullReferenceException("Prompt text atempted to be changed to null");
        promptBackground.rectTransform.sizeDelta = new Vector2(400, 300);
        TMPtext.text = text;
        TMPtext.ForceMeshUpdate();
        Bounds bounds = TMPtext.bounds;
        Debug.Log(bounds);
        Debug.Log(bounds.size);
        Debug.Log(promptBackground.rectTransform.sizeDelta);
        promptBackground.rectTransform.sizeDelta = new Vector2(bounds.size.x, bounds.size.y) + new Vector2(10, 40) ;
        TMPtext.CrossFadeAlpha(1, 0.5f, false);
        promptBackground.CrossFadeAlpha(1, 0.5f, false);
    }
/**
    <summary>
        Same as regular updatePrompt but the text fades out after the specified time.
    </summary>
    <param name="text"> The new prompt text. </param>
    <param name="fadeOutDelay"> The time in seconds till the fadeout should start. </param>
    <returns> The new text value of the prompt. </returns>
*/
    public void updatePrompt(string text, float fadeOutDelay){
        updatePrompt(text);
        StartCoroutine(fadeOut(fadeOutDelay));
    }

    
    public IEnumerator fadeOut(float fadeOutDelay){
        yield return new WaitForSeconds(fadeOutDelay + 0.5f);
        TMPtext.CrossFadeAlpha(0, 1, false);
        promptBackground.CrossFadeAlpha(0, 1, false);
    }
}
