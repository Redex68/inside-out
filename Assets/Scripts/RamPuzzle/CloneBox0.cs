using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBox0 : MonoBehaviour
{

    public GameObject cubeClone;
    public List<GameObject> all0Cubes = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        all0Cubes.Add(Instantiate(cubeClone, transform.position, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        //cloning new boxes
        if(Vector3.Distance(transform.position, all0Cubes[all0Cubes.Count - 1].transform.position) > 1.2f){
            all0Cubes.Add(Instantiate(cubeClone, transform.position, Quaternion.identity));         
        }        
    }
}
