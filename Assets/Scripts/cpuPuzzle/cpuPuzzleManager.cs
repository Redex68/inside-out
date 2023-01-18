using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpuPuzzleManager : MonoBehaviour
{

    private bool puzzleActive = false;
    private int counter;
    private int failed;
    private List<GameObject> tasks;
    private static BNG.PlayerTeleport player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<BNG.PlayerTeleport>();
        PromptScript.instance.updatePrompt("Choose the correct logic operation that was applied to get right from left objects! You can make a mistake 3 times!", 3f);
        SetupPuzzle();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleActive) {
            if(counter == 3) {
                PuzzleCleared();
            }
            else if(failed == 3) {
                PuzzleFailed();
            }

        }
        
    }

    private void SetupPuzzle()
    {

        puzzleActive = true;
        tasks = new List<GameObject>();
        for(int i = 1; i <= 5; i++) {
            string tag = "task" + i;
            GameObject obj = GameObject.FindWithTag(tag);
            tasks.Add(obj);
        }
        this.counter = 0;
        this.failed = 0;
       
    }

    public void TaskCorrect(string index) {
        string tag = "task" + index;
        GameObject currentTask = GameObject.FindWithTag(tag);
        if (tasks.Contains(currentTask)) {
            tasks.Remove(currentTask);
            string text = PromptScript.instance.getPrompt();
            this.counter++;
            PromptScript.instance.updatePrompt("Correct! Currently solved: " + counter + " / 3", 3);
            StartCoroutine(delayedPrompt(text));
            Debug.Log("Correct! Solved: " + counter + " / 3" + index);
        }
        

    }

    public void TaskInCorrect(string index) {
        GameObject currentTask = GameObject.FindWithTag(index);
        if(tasks.Contains(currentTask)) {
            string text = PromptScript.instance.getPrompt();
            this.failed++;
            PromptScript.instance.updatePrompt("Incorrect! Mistakes: " + failed + " / 3", 3);
            StartCoroutine(delayedPrompt(text));
            Debug.Log("Incorrect! Solved: " + counter + " / 3" + index);
        }
        

    }

    private static IEnumerator delayedPrompt(String text){
        yield return new WaitForSeconds(5);
        PromptScript.instance.updatePrompt(text, 3);
    }

    public void PuzzleCleared()
    {
        
        puzzleActive = false;
        PromptScript.instance.updatePrompt("Congratulations! You have beaten the puzzle!", 3);
        TransitionManager.completePuzzle();
    }

    public void PuzzleFailed() {
        puzzleActive = false;
        this.counter = 0;
        this.failed = 0;
        player.TeleportPlayer(new Vector3(-499.709991f,186.539993f,992.76001f), Quaternion.identity);
        SetupPuzzle();
    }
}
