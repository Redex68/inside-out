using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAN_ID : MonoBehaviour
{
    [SerializeField] AudioClip surprise;

    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.AttachCallbackEvent.AddListener((comp) => onAttach(comp));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onAttach(PC.Component comp)
    {
        if(comp.name == "FAN")
        {
            AudioSource.PlayClipAtPoint(surprise, transform.position);
            PromptScript.instance.updatePrompt(Localization.Loc.loc(Localization.StoryTxt.Error), 3.0f);
        }
    }
}
