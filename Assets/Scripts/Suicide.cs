using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicide : MonoBehaviour
{
    [SerializeField] float Delay = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Delay -= Time.deltaTime;
        if(Delay < 0.0f) Destroy(gameObject);
    }
}
