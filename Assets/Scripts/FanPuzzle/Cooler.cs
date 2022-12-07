using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooler : MonoBehaviour
{
    [SerializeField]
    HeatManager manager;

    [SerializeField]
    float timeToCool;

    private float coolPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        coolPerSecond = 1f / timeToCool;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        GameObject hitObject = hitInfo.collider.gameObject;

        if(hitObject.tag.Equals("HeatedComponent")){
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            manager.CoolObject(hitObject, coolPerSecond * Time.deltaTime);
        }
        else{
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }
    }
}
