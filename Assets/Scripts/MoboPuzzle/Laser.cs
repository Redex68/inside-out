using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BNG;

public class Laser : MonoBehaviour
{
    public LineRenderer lr;
    public Grabbable g;

    float[,] drawFilter;
    public float elipseFactor = 1.0f;
    public int drawSize = 10;
    public float drawSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        
        drawFilter = new float[drawSize,drawSize];
        float peak = 0;

        for(int i = 0; i < drawSize; i++)
            for(int j = 0; j < drawSize; j++)
            {
                float halfDrawSize = drawSize/2.0f;
                float a = (i - halfDrawSize) * elipseFactor;
                float b = (j - halfDrawSize) * elipseFactor;
                float val = Mathf.Max(halfDrawSize*halfDrawSize*elipseFactor - a*a - b*b, 0.0f);
                drawFilter[i,j] = Mathf.Sqrt(val);
                peak = Mathf.Max(peak, drawFilter[i,j]);
            }

        for(int i = 0; i < drawSize; i++)
            for(int j = 0; j < drawSize; j++)
                drawFilter[i,j] /= peak;
    }

    // Update is called once per frame
    void Update()
    {
        if(g.BeingHeld)
        {
            Vector3[] positions = 
            {
                transform.position,
                transform.position + transform.TransformDirection(Vector3.right) * 2.0f
            };
            lr.enabled = true;
            lr.SetPositions(positions);

            if(Input.GetKey(KeyCode.Mouse0))
            {
                lr.material.color = Color.green;
                RaycastHit hit;
                Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.right) * 2.0f);

                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.name == "TexWrite")
                    {
                        Texture2D t = hit.transform.GetComponent<TextureWrite>().tex;
                        int width = (int)   (hit.textureCoord.x * (float)t.width);
                        int height = (int)  (hit.textureCoord.y * (float)t.height);
                        drawOnTexure(t, width, height);
                    }
                }
            }
            else
            {
                lr.material.color = Color.red;
            }
        }
        else
        {
            lr.enabled = false;
        }
    }

    void drawOnTexure(Texture2D tex, int centerWidth, int centerHeight)
    {
        float halfDrawSize = drawSize / 2.0f;
        
        int beginWidth = Mathf.CeilToInt(centerWidth - halfDrawSize);
        int beginHeight = Mathf.CeilToInt(centerHeight - halfDrawSize);

        Color[] pixels = tex.GetPixels(beginWidth, beginHeight, drawSize, drawSize);

        for(int i = 0; i < drawSize; i++)
            for(int j = 0; j < drawSize; j++)
                if(pixels[i*drawSize+j].g < drawFilter[i,j])
                    pixels[i*drawSize+j].g = Mathf.Min
                    (
                        drawFilter[i,j], 
                        pixels[i*drawSize+j].g + drawFilter[i,j] * Time.deltaTime * drawSpeed
                    );


        tex.SetPixels(beginWidth, beginHeight, drawSize, drawSize, pixels);
        tex.Apply();
    }
}
