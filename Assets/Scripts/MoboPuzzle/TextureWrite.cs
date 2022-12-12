using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureWrite : MonoBehaviour
{
    public Renderer r;
    public Transform PlayerTransform;
    public ComputeShader tempShader;

    static int texWidth =   800;
    static int texHeight =  800;

    RenderTexture weldTexture {get;set;}
    public RenderTexture getWeldTexture() { return weldTexture; }

    private float heatTime = 0.0f;
    private float coolTime = 0.0f;
    private static float scaleFactor = 0.17799f;
    ComputeBuffer cb;

    // Start is called before the first frame update
    void Start()
    {
        weldTexture = new RenderTexture(texWidth, texHeight, 1);
        weldTexture.enableRandomWrite = true;
        weldTexture.filterMode = FilterMode.Bilinear;
        weldTexture.Create();

        cb = new ComputeBuffer(Laser.drawSize*Laser.drawSize, sizeof(float));
        cb.SetData(Laser.drawFilter);

        tempShader.SetBuffer(0, "drawFilter", cb);
        tempShader.SetBuffer(1, "drawFilter", cb);
        tempShader.SetBuffer(2, "drawFilter", cb);
        tempShader.SetTexture(0, "Result", weldTexture);
        tempShader.SetTexture(1, "Result", weldTexture);
        tempShader.SetTexture(2, "Result", weldTexture);

        tempShader.SetInt("drawSize", Laser.drawSize);
        tempShader.SetFloat("drawSpeed", Laser.drawSpeed);
        tempShader.SetFloat("coolSpeed", Laser.coolSpeed);
        tempShader.SetFloat("heatSpeed", Laser.heatSpeed);
        tempShader.SetInt("texWidth", texWidth);
        tempShader.SetInt("texHeight", texHeight);

        tempShader.Dispatch(2, 32, 32, 1);

        r.material.SetTexture("_BaseMap", weldTexture);
        r.material.SetFloat("_TexelWidth", 1.0f / texWidth);
        r.material.SetFloat("_TexelHeight", 1.0f / texHeight);
        r.material.SetFloat("_ScaleFactor", scaleFactor);
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, PlayerTransform.position);
        r.material.SetFloat("_PlayerDistance", dist);
        updateDeltaTime();

        if(dist < 6.0f) runTempShader();
    }

    void updateDeltaTime()
    {
        heatTime += Time.deltaTime;
        coolTime += Time.deltaTime;

        if(heatTime > 0.01f) 
        {
            tempShader.SetBool("heatStop", false);
            heatTime -= 0.01f;
        }
        else tempShader.SetBool("heatStop", true);

        if(coolTime > 0.03f)
        {
            tempShader.SetBool("coolStop", false);
            coolTime -= 0.03f;    
        }
        else tempShader.SetBool("coolStop", true);
    }

    void runTempShader()
    {
        tempShader.Dispatch(0, 32, 32, 1);

        tempShader.SetInt("beginWidth", -1000);
        tempShader.SetInt("beginHeight", -1000);
    }
}
