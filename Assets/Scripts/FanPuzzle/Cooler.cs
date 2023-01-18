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
    private AudioSource fanBlowing;
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
        fanBlowing = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if(grabbable.BeingHeld){
            if(grabbable.HeldByGrabbers[0].HandSide == BNG.ControllerHand.Left){
                if(BNG.InputBridge.Instance.LeftTrigger > 0.5){
                    checkSoundClip();
                    fireCoolingRay();
                }

                if(BNG.InputBridge.Instance.LeftTriggerDown){
                    animator.Rebind();
                    animator.speed = 1;
                    wasActivatedInLeft = true;
                }
            }
            else if(grabbable.HeldByGrabbers[0].HandSide == BNG.ControllerHand.Right){
                if(BNG.InputBridge.Instance.RightTrigger > 0.5){
                    checkSoundClip();
                    fireCoolingRay();
                }

                if(BNG.InputBridge.Instance.RightTriggerDown){
                    animator.Rebind();
                    animator.speed = .5f;
                    wasActivatedInRight = true;
                }
            }
        }

        if((BNG.InputBridge.Instance.LeftTriggerUp || !grabbable.BeingHeld) && wasActivatedInLeft){
            animator.CrossFade("Rotation stop", 0, 0);
            wasActivatedInLeft = false;
            endSoundClip();
        }

        if((BNG.InputBridge.Instance.RightTriggerUp || !grabbable.BeingHeld) && wasActivatedInRight){
            animator.CrossFade("Rotation stop", 0, 0);
            wasActivatedInRight = false;
            endSoundClip();
        }
    }

    private void checkSoundClip(){
        if(!fanBlowing.isPlaying) {
            fanBlowing.Play();
            fanBlowing.time = 1;
        }
        else if(fanBlowing.time > 25.1){
            fanBlowing.time = 1;
        }
        else if(fanBlowing.time > 25){
            fanBlowing.time = 5.1f;
        }
    }

    private void endSoundClip(){
        if(fanBlowing.time > 5) fanBlowing.time = 25.5f;
        else fanBlowing.time = 27.385f - Mathf.InverseLerp(0, 5, fanBlowing.time) * 2.5f;
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
