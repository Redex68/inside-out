using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooler : MonoBehaviour
{
    [SerializeField]
    public HeatManager manager;

    [SerializeField]
    float timeToCool;

    [SerializeField]
    float maxRange;

    private Animator animator;
    private BNG.Grabbable grabbable;

    private float coolPerSecond;
    private bool wasActivatedInLeft = false;
    private bool wasActivatedInRight = false;
    // Start is called before the first frame update
    void Start()
    {
        coolPerSecond = 1f / timeToCool;
        animator = GetComponentInChildren<Animator>();
        animator.speed = 0;

        grabbable = GetComponent<BNG.Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {

        if(grabbable.BeingHeld){
            if(grabbable.HeldByGrabbers[0].HandSide == BNG.ControllerHand.Left){
                if(BNG.InputBridge.Instance.LeftTrigger > 0.5){
                    fireCoolingRay();
                }

                if(BNG.InputBridge.Instance.LeftTriggerDown){
                    animator.Rebind();
                    animator.speed = 2;
                    wasActivatedInLeft = true;
                }
            }
            else if(grabbable.HeldByGrabbers[0].HandSide == BNG.ControllerHand.Right){
                if(BNG.InputBridge.Instance.RightTrigger > 0.5){
                    fireCoolingRay();
                }

                if(BNG.InputBridge.Instance.RightTriggerDown){
                    animator.Rebind();
                    animator.speed = 2;
                    wasActivatedInRight = true;
                }
            }
        }

        if((BNG.InputBridge.Instance.LeftTriggerUp || !grabbable.BeingHeld) && wasActivatedInLeft){
            animator.CrossFade("Rotation stop", 0, 0);
            wasActivatedInLeft = false;
        }

        if((BNG.InputBridge.Instance.RightTriggerUp || !grabbable.BeingHeld) && wasActivatedInRight){
            animator.CrossFade("Rotation stop", 0, 0);
            wasActivatedInRight = false;
        }
    }

    private void fireCoolingRay(){
        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, maxRange);
        if(hitInfo.collider){
            GameObject hitObject = hitInfo.collider.gameObject;

            //If the object that was hit was a heated component then cool it down.
            if(hitObject.tag.Equals("HeatedComponent")){
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                manager.CoolObject(hitObject, coolPerSecond * Time.deltaTime);
            }
            else{
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxRange, Color.green);
            }
        }
        else{
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxRange, Color.green);
        }
    }
}
