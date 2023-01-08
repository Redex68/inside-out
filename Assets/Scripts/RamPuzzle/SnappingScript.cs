using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingScript : MonoBehaviour
{
    [Tooltip("The hand GameObjects that contain the left hand's Grabber script that needs to be referenced")]
    public GameObject leftHand;
    [Tooltip("The hand GameObjects that contain the right hand's Grabber script that needs to be referenced")]
    public GameObject rightHand;
    public BNG.Grabber leftGrabber;
    public BNG.Grabber rightGrabber;
    public GameObject cubeInHand;
    public Vector3 cube1SnapPosition = new Vector3(-501.417f, 185.657f, 990.844f);
    public Vector3 cube2SnapPosition = new Vector3(-501.417f, 185.657f, 992.844f);
    public Vector3 cube3SnapPosition = new Vector3(-501.417f, 185.657f, 994.844f);
    public Vector3 cube4SnapPosition = new Vector3(-501.417f, 185.657f, 996.844f);
    public Vector3 cube5SnapPosition = new Vector3(-501.417f, 185.657f, 998.844f);
    public Vector3 cubeSnapRotation = new Vector3(0, 180, -38f);
    public float distance1, distance2, distance3, distance4, distance5;
    
    // Start is called before the first frame update
    void Start()
    {
        leftGrabber = leftHand.GetComponent<BNG.Grabber>();
        rightGrabber = rightHand.GetComponent<BNG.Grabber>();

        leftGrabber.onGrabEvent.AddListener(getCubeInHand);
        rightGrabber.onGrabEvent.AddListener(getCubeInHand);

        leftGrabber.onReleaseEvent.AddListener(releaseCube);  
        rightGrabber.onReleaseEvent.AddListener(releaseCube);
    }

    // Update is called once per frame
    void Update()
    {
        //snapping in box holders
        if(cubeInHand != null){
            distance1 = Vector3.Distance(cube1SnapPosition, cubeInHand.transform.position);
            distance2 = Vector3.Distance(cube2SnapPosition, cubeInHand.transform.position);
            distance3 = Vector3.Distance(cube3SnapPosition, cubeInHand.transform.position);
            distance4 = Vector3.Distance(cube4SnapPosition, cubeInHand.transform.position);
            distance5 = Vector3.Distance(cube5SnapPosition, cubeInHand.transform.position);

            if(distance1 < 0.5f){
                    cubeInHand.transform.position = cube1SnapPosition;
                    cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
                }
            else if(distance2 < 0.5){
                cubeInHand.transform.position = cube2SnapPosition;
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
            }
            else if(distance3 < 0.5){
                cubeInHand.transform.position = cube3SnapPosition;
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
            }
            else if(distance4 < 0.5){
                cubeInHand.transform.position = cube4SnapPosition;
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
            }
            else if(distance5 < 0.5){
                cubeInHand.transform.position = cube5SnapPosition;
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
            }
        }
    }

    public void getCubeInHand(BNG.Grabbable grabbable){
        cubeInHand = grabbable.gameObject;
        PromptScript.instance.updatePrompt("Drzis " + cubeInHand.name);
    }

    public void releaseCube(BNG.Grabbable grabbable){
        PromptScript.instance.updatePrompt("");
    }
}
