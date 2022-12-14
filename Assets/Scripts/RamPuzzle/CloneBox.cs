using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBox : MonoBehaviour
{

    public GameObject cubeClone;
    public List<GameObject> allCubes = new List<GameObject>();
    public int boxCounter = 1;

    // Start is called before the first frame update
    void Start()
    {
        allCubes.Add(Instantiate(cubeClone, transform.position, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, allCubes[boxCounter - 1].transform.position) > 1.2f){
            allCubes.Add(Instantiate(cubeClone, transform.position, Quaternion.identity));
            boxCounter++;            
        }
    }
}
