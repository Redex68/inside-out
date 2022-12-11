using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureWrite : MonoBehaviour
{
    public Renderer r;
    public Texture2D tex;
    public Transform PlayerTransform;

    public int texWidth = 200;
    public int texHeight = 200;

    public Color[,] texData;

    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(texWidth, texHeight);
        tex.filterMode = FilterMode.Bilinear;

        Color[] color = new Color[texWidth * texHeight];
        
        for(int i = 0; i < texWidth; i++)
            for(int j = 0; j < texHeight; j++)
                color[i * texWidth + j] = Color.black;
        
        tex.SetPixels(0, 0, texWidth, texHeight, color, 0);
        tex.Apply();
        r.material.mainTexture = tex;
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, PlayerTransform.position);
        r.material.SetFloat("_PlayerDistance", dist);
    }
}
