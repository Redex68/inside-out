using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPanel : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [Space]
    [SerializeField] private short gridSize = 5;
    [SerializeField] private float buttonForwardFac = 0.4f;
    [SerializeField] [Range(0.0f,0.2f)] private float buttonMarginPercent = 0.1f;

    private GameObject[,] gridButtons;

    // Start is called before the first frame update
    void Start()
    {
        gridButtons = new GameObject[gridSize, gridSize];


        float buttonSize = ((1.0f - buttonMarginPercent) / gridSize);
        Debug.Log(buttonSize);

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
                gridButtons[i, j] = Instantiate(button, beginButtonPos + iVec * i + jVec * j, Quaternion.identity, transform);
                gridButtons[i, j].transform.localScale = new Vector3(buttonSize, buttonSize, buttonSize);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
