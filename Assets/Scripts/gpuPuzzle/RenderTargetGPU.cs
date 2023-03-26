using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTargetGPU : MonoBehaviour
{
    [SerializeField] float CameraRadius = 3.0f;
    [SerializeField] float CameraHeight = 0.8f;
    [SerializeField] float CameraSpeed = 1.0f;
    [SerializeField] int CameraRes = 100;

    static public RenderTargetGPU Instance;

    public RenderTexture RenderTexGPU;
    public RenderTexture RenderTexGPU_Complete;

    Camera cam;

    float progress = 0.0f;

    private void Awake() 
    {
        if(Instance != null) Destroy(this);
        else Instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        RenderTexGPU = new RenderTexture(CameraRes, CameraRes, 1);
        RenderTexGPU.filterMode = FilterMode.Point;
        RenderTexGPU_Complete = new RenderTexture(1000, 1000, 1);
        cam.targetTexture = RenderTexGPU;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void advance()
    {
        progress = (progress + CameraSpeed * Time.deltaTime) % (2 * Mathf.PI);
        transform.localPosition = new Vector3(Mathf.Cos(progress), 0.0f, Mathf.Sin(progress)) * CameraRadius;
        transform.localPosition = new Vector3(transform.localPosition.x, CameraHeight, transform.localPosition.z);
        transform.rotation = Quaternion.LookRotation(transform.parent.position - transform.position, new Vector3(0,1,0));
    }

    public void onRasterComplete()
    {
        cam.targetTexture = RenderTexGPU_Complete;
    }
}
