using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpuPuzzleManager : MonoBehaviour  //Puzzle
{

    private bool puzzleActive = false;
    private GameObject player;
    private int counter;
    private GameObject[] cpuPuzzleComponents;


    // Start is called before the first frame update
    void Start()
    {
        //disable components
        cpuPuzzleComponents = GameObject.FindGameObjectsWithTag("task");
        foreach(GameObject obj in cpuPuzzleComponents) obj.SetActive(false);
        PromptScript.instance.updatePrompt("Choose the correct logic operation that was applied to get right from left objects!", 3);
        
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
        //enable components
        foreach(GameObject obj in cpuPuzzleComponents) obj.SetActive(true);
        puzzleActive = true;
        this.counter = 0;
       
    }

    private IEnumerator DelayedTeleport()
    {
        yield return new WaitForSeconds(1.5f);

        SetupPuzzle();
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(-484.75f,186f,1006.78003f), Quaternion.identity);
    }

    // public override void initPuzzle(GameObject player)
    // {
    //     this.player = player;
    //     StartCoroutine(DelayedTeleport());

    // }

    public void TaskCorrect() {
        string text = PromptScript.instance.getPrompt();
        this.counter++;
        PromptScript.instance.updatePrompt("Correct! Solved: " + counter + " / 3", 3);
        PromptScript.instance.updatePrompt(text, 3);
        Debug.Log("Correct! Solved: " + counter + " / 3");

    }

    public void TaskInCorrect() {
        string text = PromptScript.instance.getPrompt();
        PromptScript.instance.updatePrompt("Incorrect! Solved: " + counter + " / 3", 3);
        PromptScript.instance.updatePrompt(text, 3);
        Debug.Log("Incorrect! Solved: " + counter + " / 3");

    }


    public void PuzzleCleared()
    {
        
        //disable components
        foreach(GameObject obj in cpuPuzzleComponents) obj.SetActive(false);
        puzzleActive = false;
        PromptScript.instance.updatePrompt("Congratulations! You have beaten the puzzle!", 3);
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 5.142f, 0), Quaternion.identity);
        counter = 0;
    }
}
