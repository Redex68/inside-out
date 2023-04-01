using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSU_ID : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.CallbackEvent.AddListener((comp) => onAttach(comp));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onAttach(PC.Component comp)
    {
        if(comp.name == "PSU") Debug.Log("PSU Attached!");
    }
}
