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
            TMPtext = GameObject.Find("Watch").GetComponentInChildren<TMPro.TMP_Text>();
        }
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

    <returns>
        The new text value of the prompt.
    </returns>
*/
    public void updatePrompt(string text){
        if(text == null) throw new NullReferenceException("Prompt text atempted to be changed to null");
        TMPtext.text = text;
    }
}
