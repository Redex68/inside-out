using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    [SerializeField]
    string[] title;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setTitle(string txt)
    {
        GetComponent<TMP_Text>().text = txt;
    }

    void OnValidate()
    {
        if(title.Length > 1) GetComponent<TMP_Text>().text = Localization.Loc.loc(title);
    }

    void OnGUI() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        billTheBoard();
    }

    void billTheBoard()
    {
        Transform playerTf = GameObject.Find("CenterEyeAnchor").transform;
        Quaternion rot = Quaternion.LookRotation(- playerTf.position + transform.position, Vector3.up);
        transform.rotation = rot;
    }
}
