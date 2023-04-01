using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public static Fader Instance;

    Image img;

    private void Awake() {
        if(Instance != null) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTransparency(float p)
    {
        Color c = img.color;
        img.color = new Color(c.r, c.g, c.b , p);
    }

}
