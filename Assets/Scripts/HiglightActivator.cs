using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiglightActivator : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    [SerializeField]
    public List<GameObject> components;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateHighlight(BNG.Grabbable grabbable){
        GameObject component = grabbable.gameObject;
        int indx = components.IndexOf(component);
        if(indx < 0 || indx >= highlights.Count) return;


        GameObject highlight = highlights[indx];
        highlight.SetActive(true);
    }

    public void deactivateHighlight(BNG.Grabbable grabbable){
        GameObject component = grabbable.gameObject;
        int indx = components.IndexOf(component);
        if(indx < 0 || indx >= highlights.Count) return;

        GameObject highlight = highlights[indx];
        highlight.SetActive(false);
    }
}
