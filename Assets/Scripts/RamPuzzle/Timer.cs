using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool timerOn = false;
    public float timeLeft = 150f; 
    public int[] cubeSequence = new int[5];
    public bool done = false;
    public GameObject cube0, cube1;
    public Vector3[] cubeSnapPosition;
    public Vector3 cubeSnapRotation = new Vector3(0, 180, -38f);

    public void Start()
    {
        List<GameObject> all0Cubes = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().all0Cubes;
        List<GameObject> all1Cubes = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().all1Cubes;
        cube0 = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().cubeClone;
        cube1 = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().cubeClone;
        cubeSnapPosition = GameObject.Find("Snapping").GetComponent<SnappingScript>().cubeSnapPosition;

        for(int i = 0; i < all0Cubes.Count - 1; i++)
            Destroy(all0Cubes[i]);
        for(int i = 0; i < all1Cubes.Count - 1; i++)
            Destroy(all1Cubes[i]);
        for(int i = 0; i < 5; i++)
            cubeSequence[i] = Random.Range(0, 2);

        Debug.Log(cubeSequence[0] + " " + cubeSequence[1] + " " + cubeSequence[2] + " " + cubeSequence[3] + " " + cubeSequence[4]);
        
        timeLeft = 150f;
        timerOn = true;
    }

    void Update()
    {
        if(timerOn){
            if(timeLeft > 0){
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);

                bool[] rightOrder = GameObject.Find("Snapping").GetComponent<SnappingScript>().rightOrder;
                int counter = 0;
                for(int i = 0; i < 5; i++)
                    if(rightOrder[i] == true)
                        counter++;
                if(counter == 5){
                    timerOn = false;
                    done = true;
                    
                    PromptScript.instance.updatePrompt("You have successfully solved the puzzle!", 5f);
                    cubeSnapPosition = GameObject.Find("Snapping").GetComponent<SnappingScript>().cubeSnapPosition;
                    StartCoroutine(endPuzzle());
                }
            }
            else{
                timeLeft = 0;
                timerOn = false;
                if(done == false)
                    StartCoroutine(startAgain());
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

    IEnumerator endPuzzle(){
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 40; i++){
            Instantiate(i%2==0 ? cube0 : cube1, cubeSnapPosition[0], Quaternion.Euler(cubeSnapRotation));
            Instantiate(i%2==0 ? cube1 : cube0, cubeSnapPosition[1], Quaternion.Euler(cubeSnapRotation));
            Instantiate(i%2==0 ? cube0 : cube1, cubeSnapPosition[2], Quaternion.Euler(cubeSnapRotation));
            Instantiate(i%2==0 ? cube1 : cube0, cubeSnapPosition[3], Quaternion.Euler(cubeSnapRotation));
            Instantiate(i%2==0 ? cube0 : cube1, cubeSnapPosition[4], Quaternion.Euler(cubeSnapRotation));
            yield return new WaitForSeconds(0.15f);
        }
        TransitionManager.completePuzzle();
        yield return new WaitForSeconds(3f);
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("cube0")) Destroy(obj);
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("cube1")) Destroy(obj);
    }
}
