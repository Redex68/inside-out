using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_ID : MonoBehaviour
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
        if(comp.name == "CPU")
        {
            AudioSource.PlayClipAtPoint(surprise, transform.position);
            PromptScript.instance.updatePrompt("Error!\nSolve a puzzle to fix it", 3.0f);
        }
    }
}
