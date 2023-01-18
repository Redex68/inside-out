using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("suicide");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator suicide()
    {
        yield return new WaitForSeconds(3);

        Instantiate(FindObjectOfType<PlayerID>().gameObject);
    }
}
