using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class RunGameEventHandler : InteractHandler
{
    protected override void Update()
    {
        if (controller != null && isPlayerInRange && controller.IsInteractable)
        {
            controller.IsInteractable = false;
            SceneManager.LoadScene(GlobalGameManager.MiniGameName_1);
        }
    }
}
