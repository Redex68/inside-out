using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingScript : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    public Material greenBoxHolder;
    public Material redBoxHolder;
    public Material defaultBoxHolder;
    public GameObject cubeInHand;
    public Vector3[] cubeSnapPosition = new Vector3[5];
    public Vector3 cubeSnapRotation = new Vector3(0, 180, -38f);
    public float[] distance = new float[5];
    public bool[] rightOrder = new bool[5];
    public int[] cubeSequence;
    public GameObject[] holders = new GameObject[5];

    void Start()
    {
        BNG.Grabber leftGrabber = leftHand.GetComponent<BNG.Grabber>();
        BNG.Grabber rightGrabber = rightHand.GetComponent<BNG.Grabber>();

        leftGrabber.onGrabEvent.AddListener(getCubeInHand);
        rightGrabber.onGrabEvent.AddListener(getCubeInHand);

        leftGrabber.onReleaseEvent.AddListener(releaseCube);  
        rightGrabber.onReleaseEvent.AddListener(releaseCube);

        float z = 990.844f;
        for(int i = 0; i < 5; z += 2f, i++){
            cubeSnapPosition[i] = new Vector3(-501.417f, 185.657f, z);
        }

        holders[0] = GameObject.Find("BoxHolder (1)");
        holders[1] = GameObject.Find("BoxHolder (2)");
        holders[2] = GameObject.Find("BoxHolder (3)");
        holders[3] = GameObject.Find("BoxHolder (4)");
        holders[4] = GameObject.Find("BoxHolder (5)");


    }

    void Update()
    {
        Debug.Log("!!!!!");
        Debug.Log(GameObject.Find("RamPuzzleElements(Clone)").transform.position);
        List<GameObject> all0Cubes = GameObject.Find("spawnPointCube0").GetComponent<CloneBox0>().all0Cubes;
        List<GameObject> all1Cubes = GameObject.Find("spawnPointCube1").GetComponent<CloneBox1>().all1Cubes;
        bool[] snapped = new bool[5];
        for(int i = 0; i < all0Cubes.Count; i++)
            for(int j = 0; j < 5; j++)
                if(all0Cubes[i] != null && Vector3.Distance(all0Cubes[i].transform.position, cubeSnapPosition[j]) < 0.2f)
                    snapped[j] = true;

        for(int i = 0; i < all1Cubes.Count; i++)
            for(int j = 0; j < 5; j++)
                if(all1Cubes[i] != null && Vector3.Distance(all1Cubes[i].transform.position, cubeSnapPosition[j]) < 0.2f)
                    snapped[j] = true;

        for(int i = 0; i < 5; i++)
            if(snapped[i] == false) holders[i].GetComponent<Renderer>().material = defaultBoxHolder;

        if(cubeInHand != null){
            cubeSequence = GameObject.Find("Timer").GetComponent<Timer>().cubeSequence;

            for(int i = 0; i < 5; i++)
                distance[i] = Vector3.Distance(cubeSnapPosition[i], cubeInHand.transform.position);
            
            if(distance[0] < 0.25f){
                cubeInHand.transform.position = cubeSnapPosition[0];
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
                if((cubeInHand.tag == "cube0" && cubeSequence[0] == 0) || (cubeInHand.tag == "cube1" && cubeSequence[0] == 1)){
                    rightOrder[0] = true;
                    GameObject.Find("BoxHolder (1)").GetComponent<Renderer>().material = greenBoxHolder;
                }
                else{
                    rightOrder[0] = false;
                    GameObject.Find("BoxHolder (1)").GetComponent<Renderer>().material = redBoxHolder;
                }
            }
            else if(distance[1] < 0.25f){
                cubeInHand.transform.position = cubeSnapPosition[1];
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
                if((cubeInHand.tag == "cube0" && cubeSequence[1] == 0) || (cubeInHand.tag == "cube1" && cubeSequence[1] == 1)){
                    rightOrder[1] = true;
                    GameObject.Find("BoxHolder (2)").GetComponent<Renderer>().material = greenBoxHolder;
                }
                else{
                    rightOrder[1] = false;
                    GameObject.Find("BoxHolder (2)").GetComponent<Renderer>().material = redBoxHolder;
                }
            }
            else if(distance[2] < 0.25f){
                cubeInHand.transform.position = cubeSnapPosition[2];
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
                if((cubeInHand.tag == "cube0" && cubeSequence[2] == 0) || (cubeInHand.tag == "cube1" && cubeSequence[2] == 1)){
                    rightOrder[2] = true;
                    GameObject.Find("BoxHolder (3)").GetComponent<Renderer>().material = greenBoxHolder;
                }
                else{
                    rightOrder[2] = false;
                    GameObject.Find("BoxHolder (3)").GetComponent<Renderer>().material = redBoxHolder;
                }
            }
            else if(distance[3] < 0.25){
                cubeInHand.transform.position = cubeSnapPosition[3];
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
                if((cubeInHand.tag == "cube0" && cubeSequence[3] == 0) || (cubeInHand.tag == "cube1" && cubeSequence[3] == 1)){
                    rightOrder[3] = true;
                    GameObject.Find("BoxHolder (4)").GetComponent<Renderer>().material = greenBoxHolder;
                }
                else{
                    rightOrder[3] = false;
                    GameObject.Find("BoxHolder (4)").GetComponent<Renderer>().material = redBoxHolder;
                }
            }
            else if(distance[4] < 0.25){
                cubeInHand.transform.position = cubeSnapPosition[4];
                cubeInHand.transform.rotation = Quaternion.Euler(cubeSnapRotation);
                if((cubeInHand.tag == "cube0" && cubeSequence[4] == 0) || (cubeInHand.tag == "cube1" && cubeSequence[4] == 1)){
                    rightOrder[4] = true;
                    GameObject.Find("BoxHolder (5)").GetComponent<Renderer>().material = greenBoxHolder;
                }
                else{
                    rightOrder[4] = false;
                    GameObject.Find("BoxHolder (5)").GetComponent<Renderer>().material = redBoxHolder;
                }
            }
        }
    }

    public void getCubeInHand(BNG.Grabbable grabbable){
        cubeInHand = grabbable.gameObject;
    }

    public void releaseCube(BNG.Grabbable grabbable){
        
    }
}
