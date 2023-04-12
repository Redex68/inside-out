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

    static string[] ChooseLogic =
    {
        "Choose the correct logic operation!",
        "Izaberi točnu logičku operaciju!"
    };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<BNG.PlayerTeleport>();
        PromptScript.instance.updatePrompt(Localization.Loc.loc(ChooseLogic), 5f);
        SetupPuzzle();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleActive) {
            if(counter == 5) {
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

    static string[] Correct =
    {
        "Correct!\nCurrently solved: ",
        "Točno!\nTrenutno Riješeno: "
    };

    static string[] Incorrect =
    {
        "Incorrect!\nMistakes: ",
        "Netočno!\nGreške: "
    };

    public void TaskCorrect(string index) {
        string tag = "task" + index;
        GameObject currentTask = GameObject.FindWithTag(tag);
        if (tasks.Contains(currentTask)) {
            tasks.Remove(currentTask);
            string text = PromptScript.instance.getPrompt();
            this.counter++;
            PromptScript.instance.updatePrompt(Localization.Loc.loc(Correct) + counter + "/5", 3);
            // StartCoroutine(delayedPrompt(text));
        }
        

    }

    public void TaskInCorrect(string index) {
        string tag = "task" + index;
        GameObject currentTask = GameObject.FindWithTag(tag);
        if(tasks.Contains(currentTask)) {
            string text = PromptScript.instance.getPrompt();
            this.failed++;
            PromptScript.instance.updatePrompt(Localization.Loc.loc(Incorrect) + failed + "/3", 3);
            // StartCoroutine(delayedPrompt(text));
        }
    }

    private static IEnumerator delayedPrompt(String text){
        yield return new WaitForSeconds(5);
        PromptScript.instance.updatePrompt(text, 3);
    }

    public void PuzzleCleared()
    {
        
        puzzleActive = false;
        PromptScript.instance.updatePrompt(Localization.Loc.loc(Localization.StoryTxt.Completed), 3);
        TransitionManager.completePuzzle();
    }

    public void PuzzleFailed() {
        puzzleActive = false;
        this.counter = 0;
        this.failed = 0;
        player.TeleportPlayer(GameObject.Find("GPU Location").transform.position, Quaternion.identity);
        SetupPuzzle();
    }
}
