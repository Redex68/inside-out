using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpuPuzzleManager : MonoBehaviour
{

    private bool puzzleActive = false;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        
        PromptScript.instance.updatePrompt("Choose the correct logic operation that was applied to get right from left objects!", 3f);
        SetupPuzzle();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleActive) {
            if(counter == 3) {
                PuzzleCleared();
            }

        }
        
    }

    private void SetupPuzzle()
    {

        puzzleActive = true;
        this.counter = 0;
       
    }

    public void TaskCorrect() {
        string text = PromptScript.instance.getPrompt();
        this.counter++;
        PromptScript.instance.updatePrompt("Correct! Currently solved: " + counter + " / 3", 3);
        PromptScript.instance.updatePrompt(text, 3);
        Debug.Log("Correct! Solved: " + counter + " / 3");

    }

    public void TaskInCorrect() {
        string text = PromptScript.instance.getPrompt();
        PromptScript.instance.updatePrompt("Incorrect! Currently solved: " + counter + " / 3", 3);
        PromptScript.instance.updatePrompt(text, 3);
        Debug.Log("Incorrect! Solved: " + counter + " / 3");

    }


    public void PuzzleCleared()
    {
        
        puzzleActive = false;
        PromptScript.instance.updatePrompt("Congratulations! You have beaten the puzzle!", 3);
        counter = 0;
        TransitionManager.completePuzzle();
    }
}
