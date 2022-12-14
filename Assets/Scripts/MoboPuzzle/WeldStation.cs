using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using BNG;

public class WeldStation : MonoBehaviour
{
    public Lever lever;
    public TextureWrite tw;

    UnityEvent leverUp;


    // Start is called before the first frame update
    void Start()
    {
        leverUp = new UnityEvent();
        leverUp.AddListener(onLeverUp);
        lever.onLeverUp = leverUp;
    }

    // Update is called once per frame
    void Update()
    {
        if(lever.LeverPercentage < 95.0f) lever.switchedOn = false;
    }

    //Reset
    void onLeverUp()
    {
        if(!tw.cleaning && !tw.electricity && tw.dirty)
        {
            StartCoroutine(tw.runBFS());
        }
    }

}
