using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerExit : MonoBehaviour
{
    PlayerInput r_PI;
    
    private void Awake()
    {
        r_PI = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        r_PI.ActivateInput();

        r_PI.actions["Exit"].performed += onExit;
    }


    void OnDisable()
    {
        r_PI.DeactivateInput();

        r_PI.actions["Exit"].performed -= onExit;
    }

    void onExit(InputAction.CallbackContext cc)
    {
        SceneManager.LoadScene("Main Menu");
    }

}
