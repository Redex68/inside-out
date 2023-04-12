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
        Color.blue,
        Color.green
    };

    public static Dictionary<Color, string> ColorToName = new Dictionary<Color, string>()
    {
        { Color.magenta, Localization.Loc.loc(new string[]{ "Magenta", "Roza"})     },
        { Color.red,     Localization.Loc.loc(new string[]{ "Red", "Crvena"})       },
        { Color.yellow,  Localization.Loc.loc(new string[]{ "Yellow", "Žuta"})      },
        { Color.green,   Localization.Loc.loc(new string[]{ "Green", "Zelena"})     },
        { Color.blue,    Localization.Loc.loc(new string[]{ "Blue", "Plava"})       }
    };

    public static int[] ColorSolution = { 1, 4, 3 };
    public int[] ColRGB = new int[3]{ 0, 0, 0 };
    public bool colorCompleted = false;

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
        if(!colorCompleted)
        {
            ColRGB[i] = j;
            CommandPanelRaster.Instance.updateUIStatus();
        }
    }
}
