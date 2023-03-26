using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CommandPanelColor : MonoBehaviour
{
    public static CommandPanelColor Instance;

    public static Color[] Colors =
    {
        Color.magenta,
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };

    public static Dictionary<Color, string> ColorToName = new Dictionary<Color, string>()
    {
        { Color.magenta, "Magenta"  },
        { Color.red,     "Red"      },
        { Color.yellow,  "Yellow"   },
        { Color.green,   "Green"    },
        { Color.blue,    "Blue"     }
    };

    public static int[] ColorSolution = { 1, 4, 3 };
    public int[] ColRGB = new int[3]{ 0, 0, 0 };


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
                rgbRow[i].GetChild(j).GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material.color = Colors[j];
                int x = i;
                int y = j;
                rgbRow[i].GetChild(j).GetChild(0).GetChild(1).GetComponent<BNG.Button>().onButtonDown.AddListener(() => onButtonDown(x,y));
            }
    }

    void onButtonDown(int i, int j)
    {
        ColRGB[i] = j;
        CommandPanelRaster.Instance.updateUIStatus();
    }
}
