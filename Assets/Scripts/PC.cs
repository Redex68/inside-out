using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    [System.Serializable]
    struct Component
    {
        [SerializeField]
        string name;
        [SerializeField]
        public List<Vector3> snapPositions;
        [SerializeField]
        public List<Vector3> snapAngles;
        [SerializeField]
        public List<GameObject> gameObjects;
        [SerializeField]
        public List<Component> subComponents;
    }

    [SerializeField]
    List<Component> components;

    [SerializeField]
    GameObject transitionManager;

    [SerializeField]
    float positionMargin;
    [SerializeField]
    float angleMargin;

    Transform tf;


    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        snapComponents();
        
    }


    public void snapComponents()
    {
        int componentIndex = -1;
        int gameObjectIndex = -1;
        int snapPosIndex = -1;

        for (int i = 0; i < components.Count; i++)
        {
            Component comp = components[i];

            for (int j = 0; j < comp.gameObjects.Count; j++)
            {
                GameObject gameObj = comp.gameObjects[j];

                Vector3 pos = gameObj.transform.localPosition; 
                Vector3 rot = gameObj.transform.localEulerAngles;

                for (int k = 0; k < comp.snapPositions.Count; k++)
                {
                    Vector3 snapPos = comp.snapPositions[k];

                    foreach (Vector3 snapAngle in comp.snapAngles)
                    {   
                        if (Mathf.Abs(snapPos.x - pos.x) > positionMargin)
                            continue;
                        if (Mathf.Abs(snapPos.y - pos.y) > positionMargin)
                            continue;
                        if (Mathf.Abs(snapPos.z - pos.z) > positionMargin)
                            continue;

                        if (Mathf.Abs((snapAngle.x % 360) - rot.x) > angleMargin)
                            continue;
                        if (Mathf.Abs((snapAngle.y % 360) - rot.y) > angleMargin)
                            continue;
                        if (Mathf.Abs((snapAngle.z % 360) - rot.z) > angleMargin)
                            continue;

                        snapPosIndex = k;
                        gameObjectIndex = j;
                        componentIndex = i;
                        break;
                    }
                    
                    if (snapPosIndex != -1) break;
                }

                if (snapPosIndex != -1) break;
            }

            if (snapPosIndex != -1) break;
        }

        if (snapPosIndex == -1) return;

        Vector3 position = components[componentIndex].snapPositions[snapPosIndex];
        Vector3 angle = components[componentIndex].snapAngles[0];
        GameObject obj = components[componentIndex].gameObjects[gameObjectIndex];

        components[componentIndex].snapPositions.RemoveAt(snapPosIndex);
        components[componentIndex].gameObjects.RemoveAt(gameObjectIndex);
        if (components[componentIndex].snapPositions.Count == 0) 
        {
            foreach (Component subComponent in components[componentIndex].subComponents)
            {
                components.Add(subComponent);
            }
            components.RemoveAt(componentIndex);
        }

        obj.transform.localPosition = position;
        obj.transform.localEulerAngles = angle;

        if (obj.GetComponent<Rigidbody>() != null) Destroy(obj.GetComponent<Rigidbody>());
        // if (obj.GetComponent<MeshCollider>() != null) Destroy(obj.GetComponent<MeshCollider>());
        // if (obj.GetComponent<BoxCollider>() != null) Destroy(obj.GetComponent<BoxCollider>());
        if (obj.GetComponent<BNG.Grabbable>() != null) Destroy(obj.GetComponent<BNG.Grabbable>());
        
    }
}
