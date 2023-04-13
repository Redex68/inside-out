using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketBox : MonoBehaviour
{
    GameObject lid;

    // Start is called before the first frame update
    void Start()
    {
        lid = transform.Find("Lid").gameObject;
        GetComponentInChildren<ColliderCallback>().subscribe((collider) => onPacketEnter(collider));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onPacketEnter(Collider c)
    {
        
    }
}
