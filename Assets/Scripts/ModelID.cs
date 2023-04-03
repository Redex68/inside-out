using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelID : MonoBehaviour
{
    [SerializeField] GameObject particleObj; //TODO Play

    static Color completeGlassColor = new Color(0x86/255.0f, 0xE0/255.0f, 0xBE/255.0f, 0x29/255.0f);
    static float showTime = 0.8f;
    static float showTimeSpeed = 0.5f;

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

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(showElement("Internal/PowerElement"));

        if(moboCompleted) onElectricityOn();
    }

    IEnumerator showElement(string elementName)
    {
        Transform tf = transform.Find(elementName);
        GameObject obj = Instantiate(particleObj, tf.position, Quaternion.Euler(0,90,0));
        obj.transform.localScale = 1.6f * new Vector3(6.75f,6.75f,6.75f) * tf.localScale.y;

        Material mat = tf.Find("Front").GetComponent<MeshRenderer>().material;
        Color original = mat.color;

        yield return new WaitForSeconds(0.3f);

        float passed = 0.0f;
        while(passed < showTime)
        {
            mat.color = Color.Lerp(original, completeGlassColor, passed / showTime);
            passed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    void onElectricityOn()
    {

        Shader.SetGlobalInt("_PowerOn", 1);
    }
}
