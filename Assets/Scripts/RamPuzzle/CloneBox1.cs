using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBox1 : MonoBehaviour
{

    public GameObject cubeClone;
    public List<GameObject> all1Cubes = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        all1Cubes.Add(Instantiate(cubeClone, transform.position, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        //cloning new boxes
        if(Vector3.Distance(transform.position, all1Cubes[all1Cubes.Count - 1].transform.position) > 1.2f){
            all1Cubes.Add(Instantiate(cubeClone, transform.position, Quaternion.identity));
        }
    }
}
