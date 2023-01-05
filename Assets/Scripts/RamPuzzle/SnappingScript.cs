using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingScript : MonoBehaviour
{
    public Vector3 cube1SnapPosition = new Vector3(-501.417f, 185.657f, 990.844f);
    public Vector3 cube2SnapPosition = new Vector3(-501.417f, 185.657f, 992.844f);
    public Vector3 cube3SnapPosition = new Vector3(-501.417f, 185.657f, 994.844f);
    public Vector3 cube4SnapPosition = new Vector3(-501.417f, 185.657f, 996.844f);
    public Vector3 cube5SnapPosition = new Vector3(-501.417f, 185.657f, 998.844f);
    public Vector3 cubeSnapRotation = new Vector3(0, 180, -38f);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //snapping in box holders
        /*if(boxCounter > 1 && Vector3.Distance(cube1SnapPosition, allCubes[boxCounter - 2].transform.position) < 0.5f){
            allCubes[boxCounter - 2].transform.position = cube1SnapPosition;
            var rotationVector = transform.rotation.eulerAngles; 
            rotationVector.y = 180f;
            rotationVector.z = -38f;
            allCubes[boxCounter - 2].transform.rotation = Quaternion.Euler(rotationVector);
        }
        if(boxCounter > 1 && Vector3.Distance(cube2SnapPosition, allCubes[boxCounter - 2].transform.position) < 1f){
            allCubes[boxCounter - 2].transform.position = cube2SnapPosition;
            var rotationVector = transform.rotation.eulerAngles; 
            //var rotationVector = cubeSnapRotation; 
            rotationVector.y = 180f;
            rotationVector.z = -38f;
            allCubes[boxCounter - 2].transform.rotation = Quaternion.Euler(rotationVector);
        }*/
    }
}
