using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderScreenGPU : MonoBehaviour
{
    public static int RasterizeCount = 1000;
    public static bool rasterComplete = false;

    MeshRenderer mr;
    RenderTexture rt;

    bool myRasterComplete = false;

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
}