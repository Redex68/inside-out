using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public CloneBox0 cubes0Script;
    public CloneBox1 cubes1Script;
    public List<GameObject> all0Cubes;
    public List<GameObject> all1Cubes;
    public Timer timer;
    public BNG.Grabbable resetButton;
    public bool recentlyPressedButton = false;
    void Start()
    {
        cubes0Script = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>();
        cubes1Script = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        resetButton = GameObject.Find("ResetButton").GetComponent<BNG.Grabbable>();
    }

    void Update()
    {
        all0Cubes = cubes0Script.all0Cubes;
        all1Cubes = cubes1Script.all1Cubes;

        if(resetButton.BeingHeld && recentlyPressedButton == false)
            StartCoroutine(reset());

    }

    IEnumerator reset(){
        recentlyPressedButton = true;

        if((all0Cubes.Count + all1Cubes.Count) > 2){
            for(int i = 0; i < all0Cubes.Count - 1; i++)
                Destroy(all0Cubes[i]);
            for(int i = 0; i < all1Cubes.Count - 1; i++)
                Destroy(all1Cubes[i]);
        }
        timer.Start();

        yield return new WaitForSeconds(1f);

        recentlyPressedButton = false;
    }
}
