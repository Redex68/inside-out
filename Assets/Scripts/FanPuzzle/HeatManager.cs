using System.Collections.Generic;
using UnityEngine;

public class HeatManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The average time a component will need to overheat in seconds.")]
    float averageTimeTillOverheat; 

    [SerializeField]
    [Range(0.5f, 1f)]
    [Tooltip("The minimum time a component can take to overheat as a percentage of averageTimeTillOverheat.")]
    float minTimePercent;

    [SerializeField]
    [Range(1f, 2f)]
    [Tooltip("The maximum time a component can take to overheat as a percentage of averageTimeTillOverheat.")]
    float maxTimePercent;

    [SerializeField]
    [Tooltip("The maximum heat percentage a component can reach (dictates how red it will appear)")]
    float maxHeat;

    [SerializeField]
    [Tooltip("The prefab that contains a heated component's UI elements (the TextField for the name, the progress bar...)")]
    GameObject heatedComponentUIEntryPrefab;

    /// <summary> A list containing all of the active heated components' heatInfo descriptors </summary>
    private List<HeatInfo> heatInfos = new List<HeatInfo>();
    /// <summary> The UI that is attached to the watch, displays progress bars </summary>
    
    private HeatUIManager UIManager;
    void Start()
    {
        GameObject[] heatedComponents = GameObject.FindGameObjectsWithTag("HeatedComponent");

        //Initialise the heatinfos of each component
        foreach(GameObject obj in heatedComponents) setupHeatInfo(obj);
        //Initialise the UIManager
        UIManager = new HeatUIManager(heatInfos, heatedComponentUIEntryPrefab, maxHeat);
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.updateUI();
        foreach(HeatInfo heatInfo in heatInfos){
            Color newColor = heatInfo.renderer.material.color;
            //Max heat is 60%
            if(heatInfo.heat < maxHeat){
                heatInfo.heat += heatInfo.heatPerSecond * Time.deltaTime;
                newColor.a = heatInfo.heat;
                heatInfo.renderer.material.color = newColor;
            }
            else{
                //TODO: when component overheats
            }
        }
    }

/**
<summary>
    Cools an object by the specified amount.
</summary>

<param name="obj">
    The object that needs to be cooled.
</param>

<param name="coolingPercentage">
    The amount the object needs to be cooled by expressed in percentage points.
</param>

<exception cref="ArgumentException">
    If <c>obj</c> is not in the list of HeatedComponents.
</exception>
<exception cref="NullReferenceException">
    If <c>obj</c> is <c>null</c>
</exception>
*/
    public void CoolObject(GameObject obj, float coolingPercentage){
        if(obj == null) throw new System.NullReferenceException("Obj cannot be null.");

        foreach(HeatInfo info in heatInfos){
            if(info.heatedObject.Equals(obj)){
                //This is so you can overcool a componet by a bit
                info.heat = Mathf.Max(-0.2f, info.heat - coolingPercentage * maxHeat);
                return;
            }            
        }
        throw new System.ArgumentException("Provided object is not in the list of HeatedComponents of the HeatManager.");
    }

/**
<summary>
    Sets up a component's HeatInfo object.
</summary>

<param name="obj">
    The component for which the HeatInfo object is being created.
</param>

<returns>
    A newly created HeatInfo object for the given component.
</returns>
*/
    private HeatInfo setupHeatInfo(GameObject obj){
            //Every object will have a different, random speed at which they heat up.
            float actualTimeTillOverheat = averageTimeTillOverheat * Random.Range(0.5f, 2f);
            float heatPerSecond = maxHeat / actualTimeTillOverheat;
            HeatInfo info = new HeatInfo(obj, 0, heatPerSecond);
            heatInfos.Add(info);

            Color newColor = info.renderer.material.color;
            newColor.a = 0;
            info.renderer.material.color = newColor;

            return info;
    }
}

/**
<summary>
    Contains info needed for managing an object's heat. Includes a renderer
    through which the component's appearance can be changed (to look hotter),
    the object's current heat and the speed at which it heats up.
</summary>
*/
public class HeatInfo{
    public HeatInfo(GameObject heatedObject, float heat, float heatPerSecond){
        this.heatedObject = heatedObject;
        this.renderer = heatedObject.GetComponent<Renderer>();
        this.heat = heat;
        this.heatPerSecond = heatPerSecond;
    }
    public readonly Renderer renderer;
    public float heat;
    public readonly float heatPerSecond;
    public GameObject heatedObject;
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Button button;
}