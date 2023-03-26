using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CommandPanelColor : MonoBehaviour
{
    public static CommandPanelColor Instance;

    static Color[] Colors =
    {
        Color.magenta,
        Color.red,
        Color.yellow,
        Color.blue,
        Color.green
    };
    
    Transform[] rgbRow;

    void Awake() 
    {
        if(Instance != null) Destroy(this);
        else Instance = this;
    } 

    public void initColorGrid()
    {
        rgbRow = new Transform[3];
        rgbRow[0] = transform.GetChild(0);
        rgbRow[1] = transform.GetChild(1);
        rgbRow[2] = transform.GetChild(2);

        for(int i = 0; i < 3; i++)
            for(int j = 0; j < 5; j++)
            {
                rgbRow[i].GetChild(j).GetComponent<MeshRenderer>().material.color = Colors[j];
                int x = i;
                int y = j;
                rgbRow[i].GetChild(j).GetComponent<BNG.Button>().onButtonDown.AddListener(() => onButtonDown(x,y));
            }
    }

    void onButtonDown(int i, int j)
    {
        
    }
}
