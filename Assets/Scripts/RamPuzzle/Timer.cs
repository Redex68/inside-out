using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool timerOn = false;
    public float timeLeft = 30f; 
    public int[] cubeSequence = new int[5];
    public bool done = false;

    public void Start()
    {
        List<GameObject> all0Cubes = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().all0Cubes;
        List<GameObject> all1Cubes = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().all1Cubes;
        /*for(int i = 0; i < all0Cubes.Count - 1; i++)
            Destroy(all0Cubes[i]);
        for(int i = 0; i < all1Cubes.Count - 1; i++)
            Destroy(all1Cubes[i]);*/
        for(int i = 0; i < 5; i++)
            cubeSequence[i] = Random.Range(0, 2);
        Debug.Log(cubeSequence[0] + " " + cubeSequence[1] + " " + cubeSequence[2] + " " + cubeSequence[3] + " " + cubeSequence[4]);
        timeLeft = 5000f;
        timerOn = true;
    }

    void Update()
    {
        if(timerOn){
            if(timeLeft > 0){
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);

                bool[] rightOrder = GameObject.Find("PlayerController").GetComponent<SnappingScript>().rightOrder;
                int counter = 0;
                for(int i = 0; i < 5; i++)
                    if(rightOrder[i] == true)
                        counter++;
                if(counter == 5){
                    timeLeft = 0;
                    timerOn = false;
                    done = true;
                    PromptScript.instance.updatePrompt("Uspješno ste riješili zagonetku!");
                }
            }
            else{
                timeLeft = 0;
                timerOn = false;
                if(done == false)   StartCoroutine(startAgain());
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        PromptScript.instance.updatePrompt(string.Format("{0:00} : {1:00}", minutes, seconds));
    }

    IEnumerator startAgain(){
        PromptScript.instance.updatePrompt("Vrijeme isteklo! Probaj opet.");
        yield return new WaitForSeconds(3f);
        Start();
    }
}
