using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        TMPtext = GameObject.Find("Watch").GetComponentInChildren<TMPro.TMP_Text>();
    }

    public string getPrompt(){
        return TMPtext.text;
    }

    public void updatePrompt(string text){
        TMPtext.text = text;
    }
}
