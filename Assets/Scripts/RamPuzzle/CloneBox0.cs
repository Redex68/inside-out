using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBox0 : MonoBehaviour
{
    public GameObject cubeClone;
    public List<GameObject> all0Cubes = new List<GameObject>();
    private GameObject puzzleInstantiatedPrefab;

    void Awake()
    {
        puzzleInstantiatedPrefab = GameObject.Find("RamPuzzleElements(Clone)");
        GameObject cube = Instantiate (cubeClone, transform.position, Quaternion.identity);
        cube.transform.parent = puzzleInstantiatedPrefab.transform;
        all0Cubes.Add(cube);
    }

    void Update()
    {
        //cloning new boxes
        if(Vector3.Distance(transform.position, all0Cubes[all0Cubes.Count - 1].transform.position) > 1.2f){
            GameObject cube = Instantiate (cubeClone, transform.position, Quaternion.identity);
            cube.transform.parent = puzzleInstantiatedPrefab.transform;
            all0Cubes.Add(cube);
        }        
    }
}
