using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CommandPanelRaster : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] AudioClip successAudio;
    [Space]
    [SerializeField] short gridSize = 5;
    [SerializeField] float buttonForwardFac = 0.4f;
    [SerializeField] [Range(0.0f,0.2f)] float buttonMarginPercent = 0.1f;

    GameObject[,] gridButtons;
    bool[,] buttonStates;
    int buttonOnCount = 0;
    bool rasterComplete = false;
    bool colorCompleted = false;
    float audioSpeed = 0.0f;

    public static CommandPanelRaster Instance;

    AudioSource audioSource;

    void Awake()
    {
        if(Instance != null) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("LateStart", 1.0f);
    }
 
    IEnumerator LateStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        initPuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        if(!rasterComplete) 
        {
            updateRasterizer();
            audioSource.pitch = audioSpeed;
        }
        else RenderTargetGPU.Instance.advance();

        if(!colorCompleted) updateColors();

        if(colorCompleted && rasterComplete)
        {
            PromptScript.instance.updatePrompt("GPU Puzzle Completed!", 4);
            TransitionManager.completePuzzle();
        }
    }

    void initPuzzle()
    {
        initButtonGrid();
        updateUIStatus();

        CommandPanelColor.Instance.initColorGrid();
    }

    void updateRasterizer()
    {
        RenderTexture rt = RenderTargetGPU.Instance.RenderTexGPU;
        if(rt == null) return;
        int pixelCount = rt.width * rt.width;

        float buttonProgress = Mathf.Clamp(1.01f * buttonOnCount / gridSize / gridSize, 0.0f, 1.0f);

        audioSpeed = buttonProgress * 9;
        audioSpeed = Mathf.Pow(audioSpeed, 1.5f);

        buttonProgress = Mathf.Pow(buttonProgress, 5);


        RenderScreenGPU.RasterizeCount += (int)(pixelCount * buttonProgress);
        if(RenderScreenGPU.RasterizeCount > pixelCount)
        {
            RenderScreenGPU.RasterizeCount -= pixelCount;
            RenderTargetGPU.Instance.advance();
        }
    }

    public void updateUIStatus()
    {
        if(CommandPanelColor.Instance == null) return;
        var rgb = CommandPanelColor.Instance.ColRGB;
        if(rgb.Length != 3) return;

        PromptScript.instance.updatePrompt
        (
            string.Format
            (
                "Graphics Processors: {0}/{1}\nColors:\n1. {2}\n2. {3}\n3. {4}",
                buttonOnCount, 
                gridSize*gridSize, 
                CommandPanelColor.ColorToName[CommandPanelColor.Colors[rgb[0]]],
                CommandPanelColor.ColorToName[CommandPanelColor.Colors[rgb[1]]],
                CommandPanelColor.ColorToName[CommandPanelColor.Colors[rgb[2]]]
            )
        );
    }

    bool compareArrs(int[] a, int[] b)
    {
        if(a.Length != b.Length) return false;
        for(int i = 0; i < a.Length; i++) if(a[i] != b[i]) return false;
        return true;
    }

    void updateColors()
    {
        if(CommandPanelColor.Instance == null) return;
        if(compareArrs(CommandPanelColor.Instance.ColRGB, CommandPanelColor.ColorSolution))
        {
            colorCompleted = true;
            CommandPanelColor.Instance.colorCompleted = true;
        }
        
        var cols = CommandPanelColor.Instance.ColRGB;

        if(cols.Length != 3) return;

        var colR = CommandPanelColor.Colors[cols[0]];
        var colG = CommandPanelColor.Colors[cols[1]];
        var colB = CommandPanelColor.Colors[cols[2]];

        Vector3[] rgbVec = new Vector3[]
        {
            new Vector3(colR.r, colR.g, colR.b).normalized,
            new Vector3(colG.r, colG.g, colG.b).normalized,
            new Vector3(colB.r, colB.g, colB.b).normalized
        };

        RenderScreenGPU.Instance.updateColor(rgbVec);
    }

    void initButtonGrid()
    {
        gridButtons = new GameObject[gridSize, gridSize];
        buttonStates = new bool[gridSize, gridSize];

        // float buttonSizeXY = ((1.0f - buttonMarginPercent) / gridSize);
        float buttonSizeXZ = (1.0f - buttonMarginPercent);
        float buttonSizeY = 1.0f / transform.localScale.z;

        Vector3 forward = transform.forward * buttonForwardFac * transform.localScale.z;
        Vector3 beginButtonPos = transform.position + 
            //Right
            (0.5f - 0.5f / gridSize) * transform.right * transform.localScale.x + 
            //Up
            (0.5f - 0.5f / gridSize) * transform.up * transform.localScale.y + 
            //Forward
            forward;

        Vector3 iVec = - transform.right * transform.localScale.x / gridSize;
        Vector3 jVec = - transform.up * transform.localScale.y / gridSize;

        for(int i = 0; i < gridSize; i++)
            for(int j = 0; j < gridSize; j++)
            {
                //Instantiation
                gridButtons[i, j] = Instantiate(button, beginButtonPos + iVec * i + jVec * j, Quaternion.Euler(90, 0, 0), transform);
                gridButtons[i, j].transform.localScale = new Vector3(buttonSizeXZ, buttonSizeY, buttonSizeXZ);
                
                //Adding listener
                int x = i;
                int y = j;
                gridButtons[i, j].GetComponentInChildren<BNG.Button>().onButtonDown.AddListener(() => onButtonDown(x,y));

                //Setting state
                buttonStates[i, j] = false;
            }
    }

    void onButtonDown(int i, int j)
    {
        if(rasterComplete) return;

        Vector2Int[] xored = new Vector2Int[]
        {
            new Vector2Int(i, j),
            new Vector2Int(i, j+1),
            new Vector2Int(i, j-1),
            new Vector2Int(i+1, j),
            new Vector2Int(i-1, j),
        };

        foreach (Vector2Int xor in xored)
        {

            if(xor.x < 0 || xor.x >= gridSize || xor.y < 0 || xor.y >= gridSize) continue;

            if(buttonStates[xor.x, xor.y])
            {
                buttonStates[xor.x, xor.y] = false;
                gridButtons[xor.x, xor.y].transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material.color = Color.red;
                buttonOnCount--;
            }
            else
            {
                buttonStates[xor.x, xor.y] = true;
                gridButtons[xor.x, xor.y].transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material.color = Color.blue;
                buttonOnCount++;
            }

            updateUIStatus();
            if(buttonOnCount == gridSize*gridSize) onRasterComplete();
        }
    }

    void onRasterComplete()
    {
        rasterComplete = true;
        RenderScreenGPU.rasterComplete = true;
        RenderTargetGPU.Instance.onRasterComplete();

        audioSource.Stop();
        audioSource.pitch = 1.0f;
        audioSource.clip = successAudio;
        audioSource.PlayOneShot(successAudio);
    }
}
