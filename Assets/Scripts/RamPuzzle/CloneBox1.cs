using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBox1 : MonoBehaviour
{
    public GameObject cubeClone;
    public List<GameObject> all1Cubes = new List<GameObject>();
    private GameObject puzzleInstantiatedPrefab;

    void Awake()
    {
        puzzleInstantiatedPrefab = GameObject.Find("RamPuzzleElements(Clone)");
        GameObject cube = (Instantiate (cubeClone, transform.position, Quaternion.identity, transform));
        cube.transform.parent = puzzleInstantiatedPrefab.transform;
        all1Cubes.Add(cube);
    }

    void Update()
    {
        //cloning new boxes
        if(Vector3.Distance(transform.position, all1Cubes[all1Cubes.Count - 1].transform.position) > 1.2f){
            GameObject cube = Instantiate (cubeClone, transform.position, Quaternion.identity, transform);
            cube.transform.parent = puzzleInstantiatedPrefab.transform;
            all1Cubes.Add(cube);
        }
    }
}
