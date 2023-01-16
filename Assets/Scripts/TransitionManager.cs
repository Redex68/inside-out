using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class TransitionManager : MonoBehaviour{
    public struct Puzzle
    {
        public String PuzzleName;
        public GameObject PuzzleObject;
        public Vector3 PuzzleStartPos;
        public bool PuzzleCompleted;
    }

    //Triba settat u editoru
    //Soba -503.41f, 185.5f, 994.549f
    //Mini soba - 0, 2.142f, 0
    public static Vector3 SpawnPosition = new Vector3(0, 2.142f, 0);

    //TREBA SERIALIZAT 
    public static Dictionary<String, Puzzle> Puzzles;

    static PC.Component currentPuzzleComponent;
    static GameObject currentPuzzleInstance = null;

    public static void startPuzzle(PC.Component comp)
    {
        if(!Puzzles.ContainsKey(comp.name)) throw new Exception("Komponenta nema puzlu");

        Task.Delay(1500).ContinueWith(x => 
        {
            currentPuzzleComponent = comp;
            currentPuzzleInstance = Instantiate(Puzzles[currentPuzzleComponent.name].PuzzleObject);
            FindObjectOfType<BNG.PlayerTeleport>().TeleportPlayer(Puzzles[currentPuzzleComponent.name].PuzzleStartPos, Quaternion.identity); 
        });
    }

    public static void quitPuzzle()
    {
        Task.Delay(1500).ContinueWith(x => 
        {
            FindObjectOfType<PlayerID>().transform.position = SpawnPosition; 
            Destroy(currentPuzzleInstance);

            //Resetting component (Adding back to component list, reseting positions, adding physics, adding grabbable)
            PC.components.Add(new PC.Component(currentPuzzleComponent));
            for(int i = 0; i < currentPuzzleComponent.gameObjects.Count; i++)
            {
                currentPuzzleComponent.gameObjects[i].transform.position = PC.defaultComponentPositions[currentPuzzleComponent.name][i];
                currentPuzzleComponent.gameObjects[i].AddComponent<Rigidbody>();
                currentPuzzleComponent.gameObjects[i].GetComponent<BNG.Grabbable>().enabled = true;
            }
        });
    }

    public static void completePuzzle()
    {
        Task.Delay(1500).ContinueWith(x =>
        {
            Puzzle currentPuzzle = Puzzles[TransitionManager.currentPuzzleComponent.name];
            currentPuzzle.PuzzleCompleted = true;

            foreach(var comp in currentPuzzleComponent.subComponents) PC.components.Add(comp);

            if(PC.components.Count > 0) FindObjectOfType<BNG.PlayerTeleport>().TeleportPlayer(SpawnPosition, Quaternion.identity);
            else
            {
                //Game finished, TODO: Perkan
            }
        });
    }

}