using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUSB : MonoBehaviour
{
    [SerializeField] AudioClip usbConnected;
    [SerializeField] GameObject Controller;

    static AudioClip usbConnectedInstance;

    static int attachCount = 0;
    static float attachOffset = 0.012f;

    bool completed = false;
    float lightInterval = 0.0f;
    int dir = 1;

    Material lightMat;

    void Awake()
    {
        if(usbConnectedInstance == null && usbConnected != null) usbConnectedInstance = usbConnected;
    }

    // Start is called before the first frame update
    void Start()
    {
        lightMat = Controller.GetComponentInChildren<ControllerLight>().GetComponent<MeshRenderer>().material;
        lightMat.color = Color.red;
        TransitionManager.CallbackEvent.AddListener((comp) => onAttach(comp));
    }

    // Update is called once per frame
    void Update()
    {
        if(completed) updateMat();
    }

    void updateMat()
    {
        lightInterval += Time.deltaTime * dir;
        if(lightInterval > 1.0f)
        {
            dir *= -1;
            lightMat.color = Color.green;
        }
        else if(lightInterval < 0.0f)
        {
            dir *= -1;
            lightMat.color = Color.black;
        }
    }

    void onAttach(PC.Component comp)
    {
        if(this.name == comp.gameObjects[0].name) 
        {
            completed = true;
            AudioSource.PlayClipAtPoint(usbConnectedInstance, transform.position);
            PromptScript.instance.updatePrompt("USB connected successfuly!", 3.0f);
        }
    }
}
