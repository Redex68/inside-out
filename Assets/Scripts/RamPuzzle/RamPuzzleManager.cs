using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamPuzzleManager : Puzzle
{
    public static RamPuzzleManager Instance {get; private set;}
    private GameObject player;
    private GameObject puzzleInstance;
    public GameObject[] ramComponents;
    private void Awake(){
        if(Instance != null && Instance != this) Destroy(this);
        else {
            Instance = this;
        }

        ramComponents = GameObject.FindGameObjectsWithTag("RamElements");
        foreach(GameObject obj in ramComponents) obj.SetActive(false);
    }

    public override void initPuzzle(GameObject player)
    {
        this.player = player;
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(-498.33f, 188.5f, 991.3f), Quaternion.identity);
        foreach(GameObject obj in ramComponents) obj.SetActive(true);
    }

    public void completePuzzle(){
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("cube0")) Destroy(obj);
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("cube1")) Destroy(obj);
        foreach(GameObject obj in ramComponents) Destroy(obj);
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 5f, 0), Quaternion.identity);
    }
}
