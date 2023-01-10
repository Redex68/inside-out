using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
    <summary>
        A manager used to control component highlights. Adds listeners to the
        onGrabEvent and onReleaseEvent events from the player's hands.
    </summary>
*/

public class HiglightActivator : MonoBehaviour
{
    [Tooltip("The hand GameObjects that contain the left hand's Grabber script that needs to be referenced")]
    public GameObject leftHand;
    [Tooltip("The hand GameObjects that contain the right hand's Grabber script that needs to be referenced")]
    public GameObject rightHand;

    [Tooltip("A list of all the PC components that can be grabbed and placed.")]
    [SerializeField]
    public List<GameObject> components;

    [Tooltip("A list of all the highlight GameObjects. Must be in the same order as the components list (i.e. the coresponding highlight GameObject must be at the same index as the Grabbable GameObject in the Components list).")]
    [SerializeField]
    public List<GameObject> highlights;
    // Start is called before the first frame update
    void Start()
    {
        BNG.Grabber leftGrabber = leftHand.GetComponent<BNG.Grabber>();
        BNG.Grabber rightGrabber = rightHand.GetComponent<BNG.Grabber>();
        leftGrabber.onGrabEvent.AddListener(activateHighlight);
        rightGrabber.onGrabEvent.AddListener(activateHighlight);
        leftGrabber.onReleaseEvent.AddListener(deactivateHighlight);  
        rightGrabber.onReleaseEvent.AddListener(deactivateHighlight);  
    }

/**
    <summary>
        Activates the highlight that coresponds to the component that was grabbed.
    </summary>
    <exception cref="IndexOutOfRangeException">
        if the grabbed component exists in the components list but the coresponding index
        is out of range for the higlights list
    </exception>
    <exception cref="NullReferenceException">
        if the grabbed component exists in the components list but the coresponding highlight
        GameObject is null in the highlights list
    </exception>
*/
    public void activateHighlight(BNG.Grabbable grabbable){
        GameObject component = grabbable.gameObject;
        int indx = components.IndexOf(component);
        if(indx < 0) return;
        if(indx >= highlights.Count)
            throw new IndexOutOfRangeException("Missing the coresponding highlight GameObject for component " + component);

        GameObject highlight = highlights[indx];
        if(highlight == null)
            throw new NullReferenceException("Missing the coresponding highlight GameObject for component " + component);

        highlight.SetActive(true);
    }

/**
    <summary>
        Deactivates the highlight that coresponds to the component that was released.
    </summary>
    <exception cref="IndexOutOfRangeException">
        if the released component exists in the components list but the coresponding index
        is out of range for the higlights list
    </exception>
    <exception cref="NullReferenceException">
        if the released component exists in the components list but the coresponding highlight
        GameObject is null in the highlights list
    </exception>
*/
    public void deactivateHighlight(BNG.Grabbable grabbable){
        GameObject component = grabbable.gameObject;
        int indx = components.IndexOf(component);
        if(indx < 0 || indx >= highlights.Count) return;
        if(indx >= highlights.Count)
            throw new IndexOutOfRangeException("Missing the coresponding highlight GameObject for component " + component);

        GameObject highlight = highlights[indx];
        if(highlight == null)
            throw new NullReferenceException("Missing the coresponding highlight GameObject for component " + component);
            
        highlight.SetActive(false);
    }
}
