using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderScreenGPU : MonoBehaviour
{
    public static RenderScreenGPU Instance;

    public static int RasterizeCount = 1000;
    public static bool rasterComplete = false;

    MeshRenderer mr;
    RenderTexture rt;

    bool myRasterComplete = false;

    void Awake()
    {
        if(Instance != null) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1.00f);

        rt = RenderTargetGPU.Instance.RenderTexGPU;
        mr.material.SetTexture("_BaseMap", rt);
        mr.material.SetInt("_TextureRes", rt.width);
    }

    // Update is called once per frame
    void Update()
    {
        updateScreen();
    }

    void updateScreen()
    {
        if(rt == null) return;
        
        if(!rasterComplete)
        {
            mr.material.SetInt("_RasterizeCount", RasterizeCount);
        }
        else
        {
            if(!myRasterComplete)
            {
                mr.material.SetInt("_RasterComplete", 1);
                mr.material.SetTexture("_BaseMap", RenderTargetGPU.Instance.RenderTexGPU_Complete);
                myRasterComplete = true;
            }
        }
    }

    public void updateColor(Vector3[] cols)
    {
        if(rt == null) return;

        mr.material.SetVector("_ColorR", cols[0]);
        mr.material.SetVector("_ColorG", cols[1]);
        mr.material.SetVector("_ColorB", cols[2]);
    }
}