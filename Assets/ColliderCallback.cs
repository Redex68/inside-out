using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCallback : MonoBehaviour
{
    UnityEngine.Events.UnityEvent<Collider> callback = new UnityEngine.Events.UnityEvent<Collider>();

    public void subscribe(UnityEngine.Events.UnityAction<Collider> action)
    {
        callback.AddListener(action);
    }

    void OnTriggerEnter(Collider other) 
    {
        callback.Invoke(other);
    }
}
