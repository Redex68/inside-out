using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public List<GameObject> all0Cubes;
    public List<GameObject> all1Cubes;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        all0Cubes = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().all0Cubes;
        all1Cubes = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().all1Cubes;

        Debug.Log(all0Cubes.Count + " " + all1Cubes.Count);
        if(GameObject.Find("ResetButton").GetComponent<BNG.Grabbable>().BeingHeld && (all0Cubes.Count + all1Cubes.Count) > 2){
            for(int i = 0; i < all0Cubes.Count - 1; i++){
                Destroy(all0Cubes[i]);
            }
            for(int i = 0; i < all1Cubes.Count - 1; i++){
                Destroy(all1Cubes[i]);
            }
        }

        //reset time (ili samo da se maknu kocke?)
    }
}
