using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [ExecuteAlways]
public class AutoSizeUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnValidate() 
    {
        Debug.Log("Resizing");
        autoResize(); 
    }

    void autoResize()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<TMP_Text>().preferredWidth);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<TMP_Text>().preferredHeight);
    }
}
