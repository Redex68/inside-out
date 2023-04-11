using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    float delay = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delay = Mathf.Max(delay - Time.deltaTime, 0.0f);
        if(delay > 0.0f) return;

        if(Input.GetKey(KeyCode.P))
        {
            GetComponentInChildren<BNG.PlayerTeleport>().TeleportPlayer(GameObject.Find("GPU Location").transform.position, Quaternion.identity);
        }
        else if(Input.GetKey(KeyCode.O))
        {
            TransitionManager.completePuzzle();
            delay += 1.0f;
        }
    }
}
