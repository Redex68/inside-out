using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Localization;

public class ModelID : MonoBehaviour
{
    [SerializeField] GameObject particleObj;
    [SerializeField] AudioClip particleSFX;

    static Color completeGlassColor = new Color(0x86/255.0f, 0xE0/255.0f, 0xBE/255.0f, 0x29/255.0f);
    static float showTime = 0.8f;
    static float showTimeSpeed = 0.5f;

    bool resetted = false;

    bool moboCompleted = false;
    bool psuCompleted = false;
    bool memoryCompleted = false;
    bool cloudCompleted = false;
    bool bcCompleted = false;
    bool rcCompleted = false;
    bool mouseCompleted = false;
    bool kbCompleted = false;
    bool gpuCompleted = false;
    bool cpuCompleted = false;
    bool outputCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        localize();

        TransitionManager.CompleteCallbackEvent.AddListener((comp) => StartCoroutine(onComplete(comp)));
        TransitionManager.AttachCallbackEvent.AddListener((comp) => StartCoroutine(onAttach(comp)));

        transform.Find("Wires/PSU1").GetComponent<MeshRenderer>().sharedMaterial.SetInt("_PowerOn", 0);
        onElectricityOn();
    }

    void localize()
    {
        Action<string, string[]> put = new Action<string, string[]>
        (
            (path, txt) => Loc.locObj(transform.Find(path), txt)
        );

        Action<string, string[]> locElement = new Action<string, string[]>
        (
            (path, txt) => Loc.locObj(transform.Find(path + "/Title"), txt)
        );
        
        put("InputElements/Title", StoryTxt.UserInput);
        put("OutputElements/Title", StoryTxt.Output);
        put("Internal/Title", StoryTxt.ComputerResources);

        locElement("InputElements/RCElement", StoryTxt.RedController);
        locElement("InputElements/BCElement", StoryTxt.BlueController);
        locElement("InputElements/MouseElement", StoryTxt.Mouse);
        locElement("InputElements/KeyboardElement", StoryTxt.Keyboard);
        locElement("OutputElements/GameElement", StoryTxt.Output);
        locElement("Internal/RasterElement", StoryTxt.Rasterization);
        locElement("Internal/SimulationElement", StoryTxt.Simulation);
        locElement("Internal/PowerElement", StoryTxt.Electricity);
        locElement("Internal/MemoryElement", StoryTxt.Memory);
        locElement("Internal/CloudElement", StoryTxt.Network);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator onAttach(PC.Component comp)
    {
        yield return new WaitForSeconds(1.0f);

        switch (comp.name)
        {
            case "PSU": onPSUCompleted(); break;
            case "USB_KB": onKBCompleted(); break;
            case "USB_MS": onMSCompleted(); break;
            case "USB_RC": onRCCompleted(); break;
            case "USB_BC": onBCCompleted(); break;

            default: break;
        }

        onElectricityOn();
    }


    IEnumerator onComplete(PC.Component comp)
    {
        yield return new WaitForSeconds(2.0f);

        switch (comp.name)
        {
            case "MOBO": onMOBOCompleted(); break;
            case "USB": onCloudCompleted(); break;
            case "GPU": onGPUCompleted(); break;
            case "CPU": onCPUCompleted(); break;
            case "RAM": onRamCompleted(); break;
            default: break;
        }

        onElectricityOn();
    }

    void onKBCompleted()
    {
        kbCompleted = true;
        StartCoroutine(showElement("InputElements/KeyboardElement"));
    }

    void onMSCompleted()
    {
        mouseCompleted = true;
        StartCoroutine(showElement("InputElements/MouseElement"));
    }

    void onRCCompleted()
    {
        rcCompleted = true;
        StartCoroutine(showElement("InputElements/RCElement"));
    }

    void onBCCompleted()
    {
        bcCompleted = true;
        StartCoroutine(showElement("InputElements/BCElement"));
    }

    void onCloudCompleted()
    {
        cloudCompleted = true;
        StartCoroutine(showElement("Internal/CloudElement"));
    }

    void onPSUCompleted()
    {
        psuCompleted = true;
        StartCoroutine(showElement("Internal/PowerElement"));
    }

    void onMOBOCompleted()
    {
        moboCompleted = true;
        showWires();
    }

    void onGPUCompleted()
    {
        gpuCompleted = true;
        StartCoroutine(showElement("Internal/RasterElement"));
    }

    void onCPUCompleted()
    {
        cpuCompleted = true;
        StartCoroutine(showElement("Internal/SimulationElement"));
    }

    void onRamCompleted()
    {
        memoryCompleted = true;
        StartCoroutine(showElement("Internal/MemoryElement"));
    }

    IEnumerator showElement(string elementName)
    {
        AudioSource.PlayClipAtPoint(particleSFX, transform.position);

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

    void showWires()
    {
        Transform wires = transform.Find("Wires");
        

        for(int i = 0; i < wires.childCount; i++)
        {
            Transform child = wires.GetChild(i);

            // Quaternion rot = Quaternion.Euler(child.localRotation.z, child.localRotation.x, child.localRotation.y);
            GameObject obj = Instantiate(particleObj, child.position, Quaternion.identity, child);
            obj.transform.localRotation = Quaternion.Euler(0, 90, 0);
            obj.transform.localScale = 1f * new Vector3(6.75f * child.localScale.z, 6.75f * child.localScale.y,3.0f);

            Debug.Log(child.name);
        }

        AudioSource.PlayClipAtPoint(particleSFX, transform.position);
        wires.gameObject.SetActive(true);
    }

    void onElectricityOn()
    {
        Action<string> on = new Action<string>(name => transform.Find(name).GetComponent<MeshRenderer>().material.SetInt("_PowerOn", 1));
        Action<string> off = new Action<string>(name => transform.Find(name).GetComponent<MeshRenderer>().material.SetInt("_PowerOn", 0));

        if(!resetted)
        {
            resetted = true;

            off("Wires/PSU1");
            off("Wires/PSU2");
            off("Wires/PSU3");
            off("Wires/PSU4");
            off("Wires/PSU5");
            off("Wires/PSU6");
            off("Wires/PSU7");
            off("Wires/PSU8");
            off("Wires/RC_OUT");
            off("Wires/KB_OUT");
            off("Wires/MouseOUT");
            off("Wires/BC_OUT");
            off("Wires/ControllerMOBO");
            off("Wires/MemoryOUT");
            off("Wires/CloudOUT");
            off("Wires/CPU_OUT");
            off("Wires/MemoryIN");
            off("Wires/CloudIN");
            off("Wires/GPU_OUT");
        }

        if(!psuCompleted) return;
        else
        {
            on("Wires/PSU1");
            on("Wires/PSU2");
            on("Wires/PSU3");
            on("Wires/PSU4");
            on("Wires/PSU5");
            on("Wires/PSU6");
            on("Wires/PSU7");
            on("Wires/PSU8");
        }

        if(bcCompleted) 
        {
            on("Wires/BC_OUT");
            on("Wires/ControllerMOBO");
        }
        if(rcCompleted) 
        {
            on("Wires/RC_OUT");
            on("Wires/ControllerMOBO");
        }
        if(mouseCompleted) 
        {
            on("Wires/MouseOUT");
            on("Wires/ControllerMOBO");
        }
        if(kbCompleted) 
        {
            on("Wires/KB_OUT");
            on("Wires/ControllerMOBO");
        }

        if(memoryCompleted) on("Wires/MemoryOUT");
        if(cloudCompleted) on("Wires/CloudOUT");

        if(cpuCompleted)
        {
            on("Wires/CPU_OUT");
            on("Wires/MemoryIN");
            on("Wires/CloudIN");
        }

        if(gpuCompleted)
        {
            on("Wires/GPU_OUT");
        }

        if(gpuCompleted && cpuCompleted && psuCompleted && memoryCompleted && cloudCompleted && (rcCompleted || bcCompleted || mouseCompleted || kbCompleted)) updateOutput();
    }

    void updateOutput()
    {
        if(!outputCompleted)
        {
            outputCompleted = true;
            StartCoroutine(showElement("OutputElements/GameElement"));
        }

        int n = Convert.ToInt32(rcCompleted) + Convert.ToInt32(bcCompleted) + Convert.ToInt32(mouseCompleted) + Convert.ToInt32(kbCompleted);

        Action<string> off = new Action<string>(name => transform.Find(name).gameObject.SetActive(false));
        Action<string> on = new Action<string>(name => transform.Find(name).gameObject.SetActive(true));

        off("OutputElements/GameElement/Game1");
        off("OutputElements/GameElement/Game2");
        off("OutputElements/GameElement/Game23");
        off("OutputElements/GameElement/Game34a");
        off("OutputElements/GameElement/Game34b");
        off("OutputElements/GameElement/Game4a");
        off("OutputElements/GameElement/Game4b");

        if(n == 1)
        {
            on("OutputElements/GameElement/Game1");
        } 
        else if(n == 2)
        {
            on("OutputElements/GameElement/Game2");
            on("OutputElements/GameElement/Game23");
        }
        else if(n == 3)
        {
            on("OutputElements/GameElement/Game23");
            on("OutputElements/GameElement/Game34a");
            on("OutputElements/GameElement/Game34b");
        }
        else
        {
            on("OutputElements/GameElement/Game34b");
            on("OutputElements/GameElement/Game34a");
            on("OutputElements/GameElement/Game4a");
            on("OutputElements/GameElement/Game4b");
        }
    }
}
