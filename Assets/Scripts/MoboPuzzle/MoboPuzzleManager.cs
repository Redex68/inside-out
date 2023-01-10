using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoboPuzzleManager : Puzzle
{
    public static MoboPuzzleManager Instance {get; private set;}

    [SerializeField]
    public GameObject puzzlePrefab;
    private GameObject player;
    private GameObject puzzleInstance;
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
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(-503.41f, 185.022f, 994.549f), Quaternion.identity);
    }

    public void completePuzzle(){
        player.GetComponent<BNG.PlayerTeleport>().TeleportPlayer(new Vector3(0, 2.142f, 0), Quaternion.identity);
        Destroy(puzzleInstance);
    }
}
