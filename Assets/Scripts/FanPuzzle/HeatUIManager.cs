using System.Collections.Generic;
using UnityEngine;

public class HeatUIManager {
    /// <summary> A list containing all of the active heated components' heatInfo descriptors </summary>
    private List<HeatInfo> heatInfos;
    /// <summary> The UI that is attached to the watch, displays progress bars </summary>
    private Canvas watchCanvas;
    /// <summary> The image that represents the compass' needle </summary>
    private UnityEngine.UI.Image compassNeedle;
    /// <summary> The component to which the compass is pointing to </summary>
    private GameObject selectedComponent;

/// <summary>
/// The prefab for the heated component UI elements.
/// </summary>
    private GameObject heatedComponentUIEntryPrefab;
/// <summary>
/// The max heat percentage a component can reach.
/// </summary>
    private float maxHeat;

    public HeatUIManager(List<HeatInfo> heatInfos, GameObject heatedComponentUIEntryPrefab, float maxHeat){
        this.heatInfos = heatInfos;
        this.heatedComponentUIEntryPrefab = heatedComponentUIEntryPrefab;
        this.maxHeat = maxHeat;

        setupUI();
    }

/// <summary>
/// Sets up everything related to the UI (creates the progress bars, finds the compass needle, etc.)
/// </summary>
    public void setupUI(){
        watchCanvas = GameObject.Find("WatchCanvas").GetComponent<Canvas>();
        if(!watchCanvas) Debug.Log("Couldn't find watch canvas.");

        compassNeedle = watchCanvas.transform.Find("CompassNeedle").GetComponent<UnityEngine.UI.Image>();
        if(!compassNeedle) Debug.Log("Couldn't find compass needle image.");
        
        int i = 0;
        foreach(HeatInfo info in heatInfos) setupUIElement(i++, info);

        selectedComponent = heatInfos[heatInfos.Count - 1].heatedObject; 
    }

/// <summary>
/// Updates the UI elements including the progress bars and compass direction.
/// </summary>
    public void updateUI(){
        foreach(HeatInfo info in heatInfos) updateUIElement(info);
        updateCompass();
    }

    
/**
<summary>
    Sets up the UI representation of a component's heat.
</summary>

<param name="i">
    The index of the component in the list. Defines where the component's
    representation will be in the list. The list starts from the bottom and goes upwards.
</param>

<param name="info">
    The HeatInfo object attached to the component whose entry is being created.
</param>
*/
    private void setupUIElement(int i, HeatInfo info){
        GameObject newObj = GameObject.Instantiate(heatedComponentUIEntryPrefab, watchCanvas.transform, false);
        newObj.transform.SetLocalPositionAndRotation(new Vector3(0, -130 + 20 * i, 0), Quaternion.identity);

        //Attach the slider and button from the UI to the HeatInfo descriptor
        info.slider = newObj.GetComponentInChildren<UnityEngine.UI.Slider>();
        info.button = newObj.GetComponentInChildren<UnityEngine.UI.Button>();
        setupButton(info.button, info.heatedObject);

        //Set the text in the UI entry to the name of the object.
        TMPro.TMP_Text text = info.button.GetComponentInChildren<TMPro.TMP_Text>();
        text.SetText(info.heatedObject.name);
    }

/// <summary>
/// Adds listeners to the UI buttons which control the compass heading. When a
/// button is clicked the selectedComponent is set to the apropriate target
/// and the compass starts to point to it.
/// </summary>
/// <param name="button"> The button which activates the event </param>
/// <param name="target"> The new selectedComponent (where the compass should
/// point to) </param>
    private void setupButton(UnityEngine.UI.Button button, GameObject target){
        button.onClick.AddListener(() => { selectedComponent = target; });
    }


/// <summary>
/// Updates the UIElement that is associated with the given HeatInfo object.
/// Updates the progress bar.
/// </summary>
/// <param name="info"> The HeatInfo object whose associated UIElements need
/// to be updated. </param>
    private void updateUIElement(HeatInfo info){
        info.slider.value = Mathf.InverseLerp(0, maxHeat, info.heat);
        Debug.Log(info.slider.value);
    }

/// <summary>
/// Updates the direction in which the compass points
/// </summary>
    private void updateCompass(){
        float angle = calculateAngle();

        compassNeedle.transform.localEulerAngles = new Vector3(0, 0, -angle);
    }

/// <summary>
/// Calculates the angle the compass should be pointing at. The angle is calculated
/// as the angle between the ortographic projections of the UI's up vector and the
/// directional vector from the UI canvas to the selectedComponent.
/// </summary>
/// <returns></returns>
    private float calculateAngle(){
        ///Calculate directional vector (UI canvas -> selectedComponent)
        Vector3 componentPos = selectedComponent.transform.position;
        Vector3 uiPos = watchCanvas.transform.position;
        Vector3 directrion = componentPos - uiPos;
        //Ortographic projection
        directrion.y = 0;

        Vector3 uiNormal = watchCanvas.transform.up;
        //Ortographic projection
        uiNormal.y = 0;

        float angle = Vector3.SignedAngle(uiNormal.normalized, directrion.normalized, Vector3.up);

        return angle;
    }
}