using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CommandPanel : MonoBehaviour
{
    [SerializeField] GameObject button;
    [Space]
    [SerializeField] short gridSize = 5;
    [SerializeField] float buttonForwardFac = 0.4f;
    [SerializeField] [Range(0.0f,0.2f)] float buttonMarginPercent = 0.1f;

    GameObject[,] gridButtons;
    bool[,] buttonStates;
    int buttonOnCount = 0;

    // Start is called before the first frame update
    void Start()
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

        for(int i = 0; i < 5; i++)
            for(int j = 0; j < 5; j++)
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void onButtonDown(int i, int j)
    {
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
        }
    }
}
