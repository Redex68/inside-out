using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeatManager : Puzzle
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

    [SerializeField]
    [Range(0, 27)]
    [Tooltip("The number of components that will be activated at the start of the puzzle.")]
    int numberOfActiveComponents;

    [SerializeField]
    [Tooltip("The image that will represent the compass' needle.")]
    Sprite compassNeedle;

    [SerializeField]
    [Tooltip("The prefab for the cooler.")]
    GameObject coolerPrefab;

    /// <summary> A list of all possible heatedComponents </summary>
    private GameObject[] heatedComponents;
    /// <summary> A list containing all of the active heated components' HeatInfo descriptors </summary>
    private List<HeatInfo> heatInfos = new List<HeatInfo>();    
    /// <summary> The UI that is attached to the watch, displays progress bars </summary>
    private HeatUIManager UIManager;
    private bool puzzleActive = false;
    private GameObject coolerInstance;
    private GameObject player;
    void Start()
    {
        heatedComponents = GameObject.FindGameObjectsWithTag("HeatedComponent");
        foreach(GameObject obj in heatedComponents) obj.SetActive(false);
    }

    private void setupPuzzle(){
        puzzleActive = true;
        //Select a random subset of heatedComponents each time.
        GameObject[] selectedComponents = heatedComponents.OrderBy(el => Random.value).Take(numberOfActiveComponents).ToArray();

        //Disable components that weren't selected, enable components that were 
        foreach(GameObject obj in heatedComponents){
            if(!selectedComponents.Contains(obj)) obj.SetActive(false);
            else obj.SetActive(true);
        }
        //Initialise the heatinfos of each component
        foreach(GameObject obj in selectedComponents) setupHeatInfo(obj);

        //Initialise the UIManager
        UIManager = new HeatUIManager(heatInfos, heatedComponentUIEntryPrefab, maxHeat, compassNeedle);
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleActive){
            UIManager.updateUI();
            foreach(HeatInfo heatInfo in heatInfos){
                Color newColor = heatInfo.renderer.material.color;
                if(heatInfo.heat < maxHeat){
                    heatInfo.heat += heatInfo.heatPerSecond * Time.deltaTime;
                    newColor.a = heatInfo.heat;
                    heatInfo.renderer.material.color = newColor;
                }
                else{
                    failPuzzle();
                    break;
                }
            }
        }
    }

/// <summary>
/// Called when the puzzle has been completed successfully.
/// </summary>
    private void puzzleCleared(){
        puzzleActive = false;
        UIManager.clearUI();
        heatInfos.Clear();
        Destroy(coolerInstance);
        PromptScript.instance.updatePrompt("Congratulations! You have beaten the puzzle.", 5);
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 2.142f, 0), Quaternion.identity);
    }

/// <summary>
/// Called when the player has failed the puzzle.
/// </summary>
    private void failPuzzle(){
        PromptScript.instance.updatePrompt("Don't let the components overheat!", 3);
        resetPuzzle();
    }

/// <summary>
/// Resets the puzzle, clearing the UI and selecting new components.
/// </summary>
    private void resetPuzzle(){
        UIManager.clearUI();
        heatInfos.Clear();
        setupPuzzle();
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
                if(info.heat < 0) objectFullyCooledDown(info);
                
                //The puzzle has been cleared
                if(heatInfos.Count == 0) puzzleCleared();

                return;
            }
        }
        throw new System.ArgumentException("Provided object is not in the list of HeatedComponents of the HeatManager.");
    }

/// <summary>
/// Called when an object has been fully cooled down. Disables the heat component of the
/// object and prints a prompt that the component has successfully been cooled.
/// </summary>
/// <param name="info"> The HeatInfo descriptor of the heated object. </param>
    private void objectFullyCooledDown(HeatInfo info){
        heatInfos.Remove(info);
        PromptScript.instance.updatePrompt("Completely cooled down " + info.heatedObject.name.ToLower() + "!", 3);
        UIManager.removeUIEntry(info);
        info.heatedObject.SetActive(false);
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

    public override void initPuzzle(GameObject player)
    {
        this.player = player;
        StartCoroutine(delayedTeleport());
    }

    private IEnumerator delayedTeleport(){
        yield return new WaitForSeconds(1.5f);

        setupPuzzle();
        coolerInstance = Instantiate(coolerPrefab, new Vector3(-503, 183.15f, 995), Quaternion.identity);
        coolerInstance.GetComponent<Cooler>().manager = this;
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(-503.41f, 184.022f, 994.549f), Quaternion.identity);
    }
}

/**
<summary>
    Contains info needed for managing an object's heat. Includes a renderer
    through which the component's appearance can be changed (to look hotter),
    the object's current heat and the speed at which it heats up. Also contains
    references to the component's associated UI elements.
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