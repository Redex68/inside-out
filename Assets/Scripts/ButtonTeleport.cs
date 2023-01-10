using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTeleport : MonoBehaviour
{

    public GameObject player;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            player.transform.position = new Vector3(-504.106f, 183.744f, 993.669f);
            player.transform.rotation = Quaternion.Euler(0,-175,0);
        }
        print("Here");
    }
}
