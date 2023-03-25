using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[ExecuteAlways]
public class MonitorDisplay : MonoBehaviour
{
    public BNG.Button resetButton;
    public TMPro.TextMeshPro monitorText;
    
    public Transform firstLerpTransform;
    public Transform secondLerpTransform;
    public float lerpSpeedFactor = 0.25f;

    Camera startPosCamera;
    Transform playerEyeTransform;
    RenderTexture rt;
    MeshRenderer r;

    bool isResetting = false;
    float resetTime = 0.0f;
    // float firstLerpTime = 0.0f;
    // float secondLerpTime = 0.0f;
    float lerpDelay = 0.0f;

    Vector3 firstLerpBeginPos;
    Quaternion firstLerpBeginRot;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<MeshRenderer>();

        Resolution res = Screen.currentResolution;
        rt = new RenderTexture(1832, 1920, 1);
        rt.antiAliasing = 4;

        startPosCamera = FindObjectOfType<StartPosCamera>().GetComponent<Camera>();
        playerEyeTransform = FindObjectOfType<PlayerCameraTransform>().transform;
        startPosCamera.targetTexture = rt;
        r.sharedMaterial.SetTexture("_BaseMap", rt);
        r.sharedMaterial.SetFloat("_ResetTime", 0.0f);

        UnityEvent ev = new UnityEvent();
        ev.AddListener(() => 
        {
            isResetting = true; 
            firstLerpBeginPos = playerEyeTransform.position;
            firstLerpBeginRot = playerEyeTransform.rotation;
        });

        resetButton.onButtonDown = ev;
    }

    // Update is called once per frame
    void Update()
    {
        resetGame();
    }

    void resetGame()
    {
        if(isResetting)
        {
            
            // if(firstLerpTime < 1.0f)
            // {
            //     firstLerpTime += Time.deltaTime * lerpSpeedFactor;

            //     playerEyeTransform.position = Vector3.Slerp(firstLerpBeginPos, firstLerpTransform.position, firstLerpTime);
            //     playerEyeTransform.rotation = Quaternion.Slerp(firstLerpBeginRot, firstLerpTransform.rotation, firstLerpTime);
            // }
            // else 
            if(resetTime < 1.0f)
            {
                resetTime += Time.deltaTime * lerpSpeedFactor;

                monitorText.alpha = Mathf.Max(0.0f, 1.0f - resetTime);
                r.sharedMaterial.SetFloat("_ResetTime", resetTime);
            }
            // else if(secondLerpTime < 1.0f)
            // {
            //     secondLerpTime += Time.deltaTime * lerpSpeedFactor;

            //     playerEyeTransform.position = Vector3.Slerp(firstLerpTransform.position, secondLerpTransform.position, secondLerpTime);
            //     playerEyeTransform.rotation = Quaternion.Slerp(firstLerpTransform.rotation, secondLerpTransform.rotation, secondLerpTime);

            //     Debug.Log("Second lerp");
            // }
            else if(lerpDelay < 2.0f)
            {
                if(lerpDelay == 0.0f) {
                    Debug.Log("Reset");
                    resetPC();
                    TransitionManager.resetManager();
                }
                lerpDelay += Time.deltaTime * lerpSpeedFactor;
            }
            else
            {
                reposition();

                // firstLerpTime = 0.0f;
                // secondLerpTime = 0.0f;
                lerpDelay = 0.0f;
                resetTime = 0.0f;

                monitorText.alpha = 1.0f;
                r.sharedMaterial.SetFloat("_ResetTime", 0.0f);

                isResetting = false;
            }
        }
    }


    void reposition()
    {
        playerEyeTransform.localPosition = Vector3.zero;
        playerEyeTransform.localRotation = Quaternion.identity;
        TransitionManager.teleport(new Vector3(0, 3.2f, 0), Quaternion.Euler(0,-90,0), 0.5f);
    }

    void resetPC()
    {
        string[] startingComponents = { "PSU", "MOBO", "FAN" };

        PC.Instance.components.Clear();
        foreach (var startComp in startingComponents)
            PC.Instance.components.Add(new PC.Component(PC.Instance.defaultComponents[startComp]));

        //Resetting component (Adding back to component list, reseting positions, adding physics, adding grabbable)
        foreach (var entry in PC.Instance.defaultComponents)
        {
            for(int i = 0; i < PC.Instance.defaultComponentPositions[entry.Key].Count; i++)
            {
                entry.Value.gameObjects[i].transform.position = PC.Instance.defaultComponentPositions[entry.Key][i] + new Vector3(0,2,0);

                if(PC.Instance.defaultComponents[entry.Key].gameObjects[i].GetComponent<Rigidbody>() == null)
                    PC.Instance.defaultComponents[entry.Key].gameObjects[i].AddComponent<Rigidbody>();
                
                if(PC.Instance.defaultComponents[entry.Key].gameObjects[i].GetComponent<BNG.Grabbable>() != null)
                {
                    Destroy(PC.Instance.defaultComponents[entry.Key].gameObjects[i].GetComponent<BNG.Grabbable>());
                    PC.Instance.defaultComponents[entry.Key].gameObjects[i].AddComponent<BNG.Grabbable>();
                }
            }
        }
    }

}
