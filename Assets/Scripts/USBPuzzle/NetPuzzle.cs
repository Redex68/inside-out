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
    int successCount = 0;
    int neededCount = 10;

    // Start is called before the first frame update
    void Start()
    {
        initPacketBoxes();
        dropNewBox();
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

    void dropNewBox()
    {
        resetPacketBoxes();
        packetInstance = Instantiate(packet, transform);
        SrcDst srcDst = getSrcDst();
        
        packetInstance.GetComponentInChildren<TMPro.TMP_Text>().text = string.Format
        (
            "To: {0} {1}\nFrom: {2} {3}", 
            graph[srcDst.dst].current,
            Random.Range(1, 50),
            graph[srcDst.src].current,
            Random.Range(1, 50)
        );

        for(int i = 0; i < graph[srcDst.src].neighbors.Count; i++)
        {
            string neighbor = graph[srcDst.src].neighbors[i];
            setPacketBox(i, neighbor);
        }
    }

    void setPacketBox(int index, string name)
    {
        packetBoxes[index].transform.Find("Lid").gameObject.SetActive(false);
        packetBoxes[index].GetComponentInChildren<TMPro.TMP_Text>().text = name;
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

    void onBoxStore(Collider c, int i)
    {
        if(c.gameObject == packetInstance)
        {
            Debug.Log(string.Format("Packet inside box {0}", i));
            Invoke("dropNewBox", 2.0f);
        }
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
