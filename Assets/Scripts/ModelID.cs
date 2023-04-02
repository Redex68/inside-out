using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelID : MonoBehaviour
{
    [SerializeField] GameObject particleObj; //TODO Play

    bool moboCompleted = false;
    bool psuCompleted = false;


    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.CompleteCallbackEvent.AddListener((comp) => onComplete(comp));
        TransitionManager.AttachCallbackEvent.AddListener((comp) => onAttach(comp));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() 
    {
        Shader.SetGlobalInt("_PowerOn", 0);
    }

    void onAttach(PC.Component comp)
    {
        switch (comp.name)
        {
            case "PSU": StartCoroutine(onPSUCompleted()); break;
            default: break;
        }
    }

    void onComplete(PC.Component comp)
    {
        switch (comp.name)
        {
            default: break;
        }
    }


    IEnumerator onPSUCompleted()
    {
        psuCompleted = true;


        if(moboCompleted)
        {
            yield return new WaitForSeconds(1.0f);

            Shader.SetGlobalInt("_PowerOn", 1);
            Material mat = GameObject.Find("PowerElement").transform.Find("Front").GetComponent<MeshRenderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
        }
    }
}
