using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class TransitionManager : MonoBehaviour{
    public static TransitionManager Instance { get; private set; }
    
    private static PC pc;

    private static BNG.PlayerTeleport player;

    [Serializable]
    public class Puzzle
    {
        [SerializeField]
        public String PuzzleName;
        [SerializeField]
        public GameObject PuzzleObject;
        [SerializeField]
        public GameObject StartPos;
        [SerializeField]
        public bool PuzzleCompleted;
    }

    //Triba settat u editoru
    //Soba -503.41f, 185.5f, 994.549f
    //Mini soba - 0, 2.142f, 0
    public Vector3 SpawnPosition = new Vector3(0, 2.142f, 0);

    [SerializeField]
    public List<Puzzle> Puzzles;

    PC.Component currentPuzzleComponent;
    GameObject currentPuzzleInstance = null;

    //A list of the larger versions of the components which are added into the miniature world
    //as the player completes puzzles
    private List<GameObject> largeComponents = new List<GameObject>();

    private List<String> componentNames = new List<String>{ 
        "FAN1",
        "FAN2",
        "PSU",
        "CABLES",
        "CPU",
        "GPU",
        "RAM1",
        "RAM2",
        "COOLER"
     };

    [SerializeField]
     public List<GameObject> locations;

    private void Awake(){
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start(){
        player = GameObject.FindObjectOfType<BNG.PlayerTeleport>();
        pc = PC.Instance;

        componentNames.ForEach(name => {
            GameObject component = GameObject.Find(name);
            largeComponents.Add(component);
            component.SetActive(false);
        });


    }

    public static void startPuzzle(PC.Component comp)
    {
        //Throws if none are found
        Puzzle puzzle = Instance.Puzzles.FirstOrDefault(puzzle => puzzle.PuzzleName == "RAM");
        if(puzzle == null) {
            Debug.Log("Komponenta nema puzlu \"" + comp.name + "\"");
            Instance.addToMiniatureWorld(comp.name);
            return;
        }

        Instance.currentPuzzleComponent = comp;

        Instance.StartCoroutine(delayedInit(puzzle));
    }

    private static IEnumerator delayedInit(Puzzle puzzle){
        //Find the coresponding puzzle object
        yield return new WaitForSeconds(1.5f);
        
        //Instantiate the puzzle and teleport the player
        Instance.currentPuzzleInstance = Instantiate(puzzle.PuzzleObject);
        //TODO: add delay and transition
        player.TeleportPlayer(puzzle.StartPos.transform.position, Quaternion.Euler(puzzle.StartPos.transform.eulerAngles));
    }

    public static void quitPuzzle()
    {
        Instance.StartCoroutine(delayedQuit());
    }

    private static IEnumerator delayedQuit(){
        yield return new WaitForSeconds(1.5f);

        player.TeleportPlayer(Instance.SpawnPosition, Quaternion.identity);
        Destroy(Instance.currentPuzzleInstance);

        //Resetting component (Adding back to component list, reseting positions, adding physics, adding grabbable)
        pc.components.Add(new PC.Component(Instance.currentPuzzleComponent));
        for(int i = 0; i < Instance.currentPuzzleComponent.gameObjects.Count; i++)
        {
            Instance.currentPuzzleComponent.gameObjects[i].transform.position = pc.defaultComponentPositions[Instance.currentPuzzleComponent.name][i];
            Instance.currentPuzzleComponent.gameObjects[i].AddComponent<Rigidbody>();
            Instance.currentPuzzleComponent.gameObjects[i].GetComponent<BNG.Grabbable>().enabled = true;
        }
    }

    public static void completePuzzle()
    {
        Instance.StartCoroutine(delayedComplete());
    }

    private static IEnumerator delayedComplete(){
        yield return new WaitForSeconds(1.5f);

        Puzzle currentPuzzle = Instance.Puzzles.FirstOrDefault(puzzle => puzzle.PuzzleName == "RAM");
        currentPuzzle.PuzzleCompleted = true;

        foreach(var comp in Instance.currentPuzzleComponent.subComponents) pc.components.Add(comp);

        Instance.addToMiniatureWorld(Instance.currentPuzzleComponent.name);
        
        Destroy(Instance.currentPuzzleInstance);      

        if(pc.components.Count > 0) player.TeleportPlayer(Instance.SpawnPosition, Quaternion.identity);
        else
        {
            //Game finished, TODO: Perkan
        }
    }

    //add component to miniature world
    //scale = 100, position(world), prefabs of components -> funkcija(imeKomponente) [switch -> instantiate]
    //ako je MOBO nista ne radi, ako je GPU ili PSU -> u start pozvat f-ju u if == null
    private void addToMiniatureWorld(string name){
        Vector3 scale = new Vector3(100, 100, 100);
        switch (name){
            case "FAN":
                largeComponents[0].SetActive(true);
                largeComponents[1].SetActive(true);
                break;

            case "PSU":
                largeComponents[2].SetActive(true);
                largeComponents[3].SetActive(true);
                break;

            case "CPU":
                largeComponents[4].SetActive(true);
                break;

            case "GPU":
                largeComponents[5].SetActive(true);
                break;

            case "RAM":
                largeComponents[6].SetActive(true);
                largeComponents[7].SetActive(true);
                break;

            case "COOLER":
                largeComponents[8].SetActive(true);
                break;

            default:
                break;
            }
    }
}