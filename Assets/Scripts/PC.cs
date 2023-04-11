using System;
using System.Collections.Generic;
using UnityEngine;

public class PC : MonoBehaviour
{
    public static PC Instance;

    public GameObject ModelInstance;
    GameObject currentModel = null;

    [System.Serializable]
    public struct Component
    {
        public Component(Component other)
        {
            this.name = other.name;
            this.snapPositions = new List<Vector3>(other.snapPositions);
            this.snapAngles = new List<Vector3>(other.snapAngles);
            this.gameObjects = new List<GameObject>(other.gameObjects);
            this.subComponents = new List<Component>(other.subComponents);
        }

        [SerializeField]
        public string name;
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
    public List<Component> components;

    public Dictionary<String, Component> defaultComponents = new Dictionary<String, Component>();
    public Dictionary<String, List<Vector3>> defaultComponentPositions = new Dictionary<String, List<Vector3>>();

    [SerializeField]
    float positionMargin;
    [SerializeField]
    float angleMargin;

    Transform tf;

    public void resetModel()
    {
        if(currentModel != null) Destroy(currentModel);
        currentModel = Instantiate(ModelInstance);
    }

    private void Awake(){
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        resetModel();
        cacheComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if(components.Count > 0) snapComponents();
    }

    void cacheComponents()
    {
        Stack<Component> componentsStack = new Stack<Component>(components);

        while (componentsStack.Count > 0)
        {
            Component comp = componentsStack.Pop();

            foreach(var c in comp.subComponents) componentsStack.Push(c);

            defaultComponents[comp.name] = new Component(comp);

            List<Vector3> componentDefaultPositions = new List<Vector3>();
            foreach(var obj in comp.gameObjects) componentDefaultPositions.Add(obj.transform.position); 

            defaultComponentPositions[comp.name] = componentDefaultPositions;
        }
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

                        if (Mathf.Abs(Mathf.DeltaAngle(snapAngle.x, rot.x)) > angleMargin)
                            continue;
                        if (Mathf.Abs(Mathf.DeltaAngle(snapAngle.y, rot.y)) > angleMargin)
                            continue;
                        if (Mathf.Abs(Mathf.DeltaAngle(snapAngle.z, rot.z)) > angleMargin)
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

        obj.transform.localPosition = position;
        obj.transform.localEulerAngles = angle;

        if (obj.GetComponent<Rigidbody>() != null) {
            Destroy(obj.GetComponent<Rigidbody>());
        }
        if (obj.GetComponent<BNG.Grabbable>() != null){
            BNG.Grabbable grab = obj.GetComponent<BNG.Grabbable>();
            grab.enabled = false;
            grab.DropItem(grab.GetPrimaryGrabber(), true, true);
        }

        if(components[componentIndex].gameObjects.Count == 0) 
        {
            String componentName = components[componentIndex].name;
            components.RemoveAt(componentIndex);
            TransitionManager.startPuzzle(defaultComponents[componentName]);
        }
    }
}
