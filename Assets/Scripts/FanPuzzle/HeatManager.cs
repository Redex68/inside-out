using System.Collections.Generic;
using UnityEngine;

public class HeatManager : MonoBehaviour
{
    [SerializeField]
    float averageTimeTillOverheat; 
    
    [SerializeField]
    List<GameObject> heatedComponents;

    [SerializeField]
    float maxHeat;

    private List<HeatInfo> heatInfos = new List<HeatInfo>();
    
    // Start is called before the first frame update
    void Start()
    {

        foreach(GameObject obj in heatedComponents){
            //Every object will have a different, random speed at which they heat up.
            float actualTimeTillOverheat = averageTimeTillOverheat * Random.Range(0.7f, 1.5f);
            float heatPerSecond = maxHeat / actualTimeTillOverheat;
            HeatInfo info = new HeatInfo(obj, 0, heatPerSecond);
            heatInfos.Add(info);

            Color newColor = info.renderer.material.color;
            newColor.a = 0;
            info.renderer.material.color = newColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(HeatInfo heatInfo in heatInfos){
            Color newColor = heatInfo.renderer.material.color;
            //Max heat is 75%
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
}

/**
<summary>
    Contains info needed for managing an object's heat. Includes a renderer
    through which the component's appearance can be changed (to look hotter),
    the object's current heat and the speed at which it heats up.
</summary>
*/
class HeatInfo{
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
}