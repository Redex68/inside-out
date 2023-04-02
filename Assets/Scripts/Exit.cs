using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    bool exiting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(!exiting && other.gameObject.GetComponentInParent<PlayerID>() != null) StartCoroutine(ExitToMenu());
    }

    IEnumerator ExitToMenu()
    {
        exiting = true;
        float fadeIn = 0.0f;
        float fadeTime = 1.0f;

        while(fadeIn < fadeTime)
        {
            fadeIn += Time.deltaTime;
            Fader.Instance.setTransparency(fadeIn / fadeTime);
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Main Menu");
    }
}
