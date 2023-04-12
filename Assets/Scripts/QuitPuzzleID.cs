using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Localization;

public class QuitPuzzleID : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        localize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void localize()
    {
        transform.Find("UICanvas/Text").GetComponent<Text>().text = Loc.loc(StoryTxt.QuitPuzzle);
    }
}
