using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    [SerializeField] public AudioClip connectionEstablished;

    public static Router Instance;

    public bool completed = false;

    float lightInterval = 0.0f;
    int dir = 1;

    Material lightMat;

    void Awake() 
    {
        if(Instance != null) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.AttachCallbackEvent.AddListener((comp) => onAttach(comp));
        lightMat = GetComponent<MeshRenderer>().sharedMaterials[4];
        lightMat.color = Color.red;
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
        if(comp.name == "USB")
        {
            AudioSource.PlayClipAtPoint(connectionEstablished, transform.position);
            PromptScript.instance.updatePrompt("Error!\nSolve a puzzle to fix it", 3.0f);
        }
    }
}
