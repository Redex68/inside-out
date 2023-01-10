using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpuPuzzleManager : MonoBehaviour  //Puzzle
{

    private bool puzzleActive = false;
    private GameObject player;
    private int counter;


    // Start is called before the first frame update
    void Start()
    {
        PromptScript.instance.updatePrompt("Choose the correct logic operation to get right object from the left object", 3);
        
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
        PromptScript.instance.updatePrompt("Correct! " + counter + " / 3", 3);
        PromptScript.instance.updatePrompt(text, 3);
        Debug.Log("Correct! " + counter + " / 3");

    }

    public void TaskInCorrect() {
        string text = PromptScript.instance.getPrompt();
        PromptScript.instance.updatePrompt("Incorrect! " + counter + " / 3", 3);
        PromptScript.instance.updatePrompt(text, 3);
        Debug.Log("Incorrect! " + counter + " / 3");

    }


    public void PuzzleCleared()
    {
        puzzleActive = false;
        PromptScript.instance.updatePrompt("Congratulations! You have beaten the puzzle!", 3);
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 5.142f, 0), Quaternion.identity);
        counter = 0;
    }
}
