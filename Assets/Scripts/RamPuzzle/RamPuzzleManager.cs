using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamPuzzleManager : Puzzle
{
    public static RamPuzzleManager Instance {get; private set;}

    [SerializeField]
    public GameObject puzzlePrefab;
    private GameObject player;
    private GameObject puzzleInstance;
    private GameObject[] ramComponents;
    private void Awake(){
        if(Instance != null && Instance != this) Destroy(this);
        else {
            Instance = this;
        }
    }

    public override void initPuzzle(GameObject player)
    {
        this.player = player;
        puzzleInstance = Instantiate(puzzlePrefab);
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(-498.33f, 186f, 991.3f), Quaternion.identity);
    }

    public void completePuzzle(){
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 2.142f, 0), Quaternion.identity);
        Destroy(puzzleInstance);
    }
}
