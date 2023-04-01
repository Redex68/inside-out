using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            GetComponentInChildren<BNG.PlayerTeleport>().TeleportPlayer(GameObject.Find("GPU Location").transform.position, Quaternion.identity);
        }
        else if(Input.GetKey(KeyCode.O))
        {
            TransitionManager.completePuzzle();
        }
    }
}
