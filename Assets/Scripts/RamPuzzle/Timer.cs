using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool timerOn = false;
    public float timeLeft = 90f; 
    public int[] cubeSequence = new int[5];
    public bool done = false;
    public GameObject cube0, cube1;
    public Vector3[] cubeSnapPosition;
    public Vector3 cubeSnapRotation = new Vector3(0, 180, -38f);
    private GameObject puzzleInstantiatedPrefab;

    public void Start()
    {
        List<GameObject> all0Cubes = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().all0Cubes;
        List<GameObject> all1Cubes = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().all1Cubes;
        cube0 = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().cubeClone;
        cube1 = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().cubeClone;
        cubeSnapPosition = GameObject.Find("Snapping").GetComponent<SnappingScript>().cubeSnapPosition;
        puzzleInstantiatedPrefab = GameObject.Find("RamPuzzleElements(Clone)");

        for(int i = 0; i < all0Cubes.Count - 1; i++)
            Destroy(all0Cubes[i]);
        for(int i = 0; i < all1Cubes.Count - 1; i++)
            Destroy(all1Cubes[i]);
        for(int i = 0; i < 5; i++)
            cubeSequence[i] = Random.Range(0, 2);

        Debug.Log(cubeSequence[0] + " " + cubeSequence[1] + " " + cubeSequence[2] + " " + cubeSequence[3] + " " + cubeSequence[4]);
        
        timeLeft = 90f;
        timerOn = true;
    }

    static string[] Solved =
    {
        "Solved ",
        "Riješeno "
    };

    static string[] TimeUp =
    {
        "Time is up! Try again!",
        "Vrijeme je isteklo! Pokušajte ponovno!"
    };

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
                    
                    PromptScript.instance.updatePrompt(Localization.Loc.loc(Localization.StoryTxt.Completed), 6f);
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

        bool[] rightOrder = GameObject.Find("Snapping").GetComponent<SnappingScript>().rightOrder;
        int counter = 0;
        for(int i = 0; i < 5; i++)
            if(rightOrder[i] == true)
                counter++;

        PromptScript.instance.updatePrompt(Localization.Loc.loc(Solved) + counter + "/5 \n" + string.Format("{0:00} : {1:00}", minutes, seconds));
    }

    IEnumerator startAgain(){
        for(int i = 5; i >= 0; i--){
            PromptScript.instance.updatePrompt(Localization.Loc.loc(TimeUp) + " \n" + string.Format("{0:00} : {1:00}", 0,  Mathf.FloorToInt(i % 60)));
            yield return new WaitForSeconds(1f);
        }
        Start();
    }

    IEnumerator endPuzzle(){
        yield return new WaitForSeconds(1f);
        // for(int i = 0; i < 40; i++){
        //     (Instantiate(i%2==0 ? cube0 : cube1, cubeSnapPosition[0], Quaternion.Euler(cubeSnapRotation), transform) as GameObject).transform.parent = puzzleInstantiatedPrefab.transform;
        //     (Instantiate(i%2==0 ? cube1 : cube0, cubeSnapPosition[1], Quaternion.Euler(cubeSnapRotation), transform) as GameObject).transform.parent = puzzleInstantiatedPrefab.transform;
        //     (Instantiate(i%2==0 ? cube0 : cube1, cubeSnapPosition[2], Quaternion.Euler(cubeSnapRotation), transform) as GameObject).transform.parent = puzzleInstantiatedPrefab.transform;
        //     (Instantiate(i%2==0 ? cube1 : cube0, cubeSnapPosition[3], Quaternion.Euler(cubeSnapRotation), transform) as GameObject).transform.parent = puzzleInstantiatedPrefab.transform;
        //     (Instantiate(i%2==0 ? cube0 : cube1, cubeSnapPosition[4], Quaternion.Euler(cubeSnapRotation), transform) as GameObject).transform.parent = puzzleInstantiatedPrefab.transform;
        //     yield return new WaitForSeconds(0.5f);
        // }
        // yield return new WaitForSeconds(3f);
        TransitionManager.completePuzzle();
    }
}
