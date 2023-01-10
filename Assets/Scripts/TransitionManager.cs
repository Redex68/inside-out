using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TransitionManager : MonoBehaviour{
    [Serializable]
    public class PuzzleInit{
        [SerializeField]
        [Tooltip("The name of the component that is associated with the puzzle.")]
        public String name;
        [SerializeField]
        [Tooltip("A reference to a puzzle manager which has the initPuzzle() method implemented.")]
        public Puzzle initializer;
    }


    [SerializeField]
    [Tooltip("A GameObject that points to the PlayerControler of a player.")]
    public GameObject player;

    [SerializeField]
    [Tooltip("A list of initializers for puzzles.")]
    public List<PuzzleInit> PuzzleInitializers;

/// <summary>
/// Starts a puzzle that is associated with the provided component name.
/// </summary>
/// <param name="name"> The name of the component that is associated with the puzzle
/// that is to be called (e.g. MOBO, FAN, CPU...) </param>
    public void startPuzzle(String name){
        PuzzleInit init = PuzzleInitializers.DefaultIfEmpty(null).FirstOrDefault(puzzleInit => puzzleInit.name == name);
        if(init != null) init.initializer.initPuzzle(player);
    }
}