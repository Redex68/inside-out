using UnityEngine;

public abstract class Puzzle: MonoBehaviour{
/// <summary>
/// Used by the transition manager to begin a puzzle. Here you should call
/// everything that is needed for your puzzle to start (including teleporting
/// the player to your puzzle location).
/// </summary>
/// <param name="player"> A reference to the player's PlayerControler GameObject </param>
    public abstract void initPuzzle(GameObject player);
}