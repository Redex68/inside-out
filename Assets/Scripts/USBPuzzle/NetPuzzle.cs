using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetPuzzle : MonoBehaviour
{
    [System.Serializable]
    struct Node
    {
        [SerializeField]
        public string current;
        [SerializeField]
        public List<string> neighbors;
    }

    [SerializeField]
    List<Node> graph;

    [SerializeField]
    GameObject packet;

    [SerializeField]
    List<PacketBox> packetBoxes; 


    GameObject packetInstance;
    SrcDst srcDstInstance;

    bool dropped = false;
    int successCount = 0;
    int neededCount = 8;

    static Dictionary<string, string[]> NamesLoc = new Dictionary<string, string[]>
    {
        { "RedField", new string[]{"RedField", "Crveno Polje"} },
        { "China Town", new string[]{"China Town", "Kineska Ulica"} },
        { "New Castle", new string[]{"New Castle", "Novi Dvorac"} },
        { "Jacks", new string[]{"Jacks", "Jakovovi"} },
        { "Big Dogs", new string[]{"Big Dogs", "Veliki psi"} },
        { "Low Place", new string[]{"Low Place", "Sitno Mjesto"} },
        { "Down Ups", new string[]{"Down Ups", "Donji Gornji"} },
        { "Small Bigs", new string[]{"Small Bigs", "Mali Veliki"} },
        { "Cat Street", new string[]{"Cat Street", "Mačja ulica"} },
        { "Capybara Street", new string[]{"Capybara Street", "Kapibara Ulica"} },
        { "Sailors", new string[]{"Sailors", "Mornarska"} },
        { "GreenHill", new string[]{"GreenHill", "Zelena Gora"} },
        { "Johns", new string[]{"Johns", "Ivanovi"} },
    };

    // Start is called before the first frame update
    void Start()
    {
        initPacketBoxes();
        dropNewBox();

        StartCoroutine(delayedStart(2.0f));
    }

    static string[] ThrowInCorrect =
    {
        "Throw packet into correct mailbox!",
        "Ubaci paket u točan poštanski sandučić!"
    };

    IEnumerator delayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        PromptScript.instance.updatePrompt(Localization.Loc.loc(ThrowInCorrect), 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    struct SrcDst
    {
        public SrcDst(int _src, int _dst) { src = _src; dst = _dst; }
        public int src;
        public int dst;
    }

    static string[] ToFrom =
    {
        "To: {0} {1}\nFrom: {2} {3}",
        "Prima: {0} {1}\nŠalje: {2} {3}"
    };

    void dropNewBox()
    {
        dropped = false;
        resetPacketBoxes();
        packetInstance = Instantiate(packet, transform);
        srcDstInstance = getSrcDst();
        
        packetInstance.GetComponentInChildren<TMPro.TMP_Text>().text = string.Format
        (
            Localization.Loc.loc(ToFrom), 
            Localization.Loc.loc(NamesLoc[graph[srcDstInstance.dst].current]),
            Random.Range(1, 50),
            Localization.Loc.loc(NamesLoc[graph[srcDstInstance.src].current]),
            Random.Range(1, 50)
        );

        for(int i = 0; i < graph[srcDstInstance.src].neighbors.Count; i++)
        {
            string neighbor = graph[srcDstInstance.src].neighbors[i];
            setPacketBox(i, neighbor);
        }
    }

    void setPacketBox(int index, string name)
    {
        packetBoxes[index].transform.Find("Lid").gameObject.SetActive(false);
        packetBoxes[index].GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Loc.loc(NamesLoc[name]);
    }

    void resetPacketBoxes()
    {
        foreach(var box in packetBoxes)
        {
            box.transform.Find("Lid").gameObject.SetActive(true);
            box.GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }

        Destroy(packetInstance);
    }

    SrcDst getSrcDst()
    {
        int i = 0;
        int j = 0;

        while(true)
        {
            while(true)
            {
                i = Random.Range(0, graph.Count);
                j = Random.Range(0, graph.Count);
                if(i != j) break;
            }
            if(graph[i].neighbors.Contains(graph[j].current) || graph[i].neighbors.Count < 2) continue;
            else break;
        }

        return new SrcDst(i, j);
    }

    void initPacketBoxes()
    {
        for(int i = 0; i < 5; i++)
        {
            int k = i;
            packetBoxes[i].GetComponentInChildren<ColliderCallback>().subscribe((collider) => onBoxStore(collider, k));
        }
    }

    static string[] CorrectMailbox =
    {
        "Correct Mailbox!\n{0}/{1}",
        "Točan sandučić!\n{0}/{1}"
    };

    static string[] IncorrectMailbox =
    {
        "Incorrect Mailbox!\n{0}/{1}",
        "Netočan sandučić!\n{0}/{1}"
    };

    void onBoxStore(Collider c, int i)
    {
        if(c.gameObject == packetInstance)
        {
            if(!dropped)
            {
                dropped = true;

                if(isCorrect(i))
                {
                    successCount += 1;

                    PromptScript.instance.updatePrompt(string.Format(Localization.Loc.loc(CorrectMailbox), successCount, neededCount));

                    // Play sound
                }
                else
                {
                    successCount -= 1;
                    successCount = successCount < 0 ? 0 : successCount;

                    PromptScript.instance.updatePrompt(string.Format(Localization.Loc.loc(IncorrectMailbox), successCount, neededCount));

                    // Play sound
                }

                if(successCount == neededCount) onComplete();
                else Invoke("dropNewBox", 2.0f);
            }
        }
    }

    bool isCorrect(int choosen)
    {
        //current, parent
        HashSet<string> visited = new HashSet<string>();
        Stack<string> s = new Stack<string>();

        s.Push(graph[srcDstInstance.src].neighbors[choosen]);

        visited.Add(graph[srcDstInstance.src].current);
        foreach (var neighbor in graph[srcDstInstance.src].neighbors)
            visited.Add(neighbor);

        int val = 10;

        while(val-- > 0 && s.Count > 0)
        {
            string curr = s.Pop();
            if(curr == graph[srcDstInstance.dst].current) return true;

            Node? currNode = null;
            foreach (var current in graph) if(current.current == curr)
            {
                currNode = current;
                break;
            }

            foreach (var neighbor in currNode.Value.neighbors)
            {
                if(!visited.Contains(neighbor)) 
                {
                    s.Push(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return false;
    }

    static string[] Connected =
    {
        "Connected to a network!",
        "Povezano na mrežu!"
    };

    void onComplete()
    {
        Router.Instance.completed = true;
        PromptScript.instance.updatePrompt(Localization.Loc.loc(Localization.StoryTxt.Completed), 3.0f);
        TransitionManager.completePuzzle();
    }
}
