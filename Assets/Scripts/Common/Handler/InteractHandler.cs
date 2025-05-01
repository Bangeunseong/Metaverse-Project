using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractHandler : MonoBehaviour
{
    [SerializeField] private LayerMask target;
    [SerializeField] private GameObject uiCanvas;

    private bool isPlayerInRange = false;
    private PlayerController controller;

    private void Start()
    {
        WorldManager.handlers.Add(this);    
    }

    protected virtual void Update()
    {
        if(controller != null && isPlayerInRange && controller.IsInteractable)
        {
            Debug.Log("Event occured!");
            controller.IsInteractable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player in range");
        if (target == (target.value | ( 1 << collision.gameObject.layer)))
        {
            uiCanvas.SetActive(true);
            PlayerController controller = Helper.GetComponent_Helper<PlayerController>(collision.gameObject);
            if (controller != null) { this.controller = controller; isPlayerInRange = true; }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPlayerInRange) { return; }

        Debug.Log("Player stayed in range");
        if (target == (target.value | (1 << collision.gameObject.layer)))
        {
            uiCanvas.SetActive(true);
            PlayerController controller = Helper.GetComponent_Helper<PlayerController>(collision.gameObject);
            if (controller != null) { this.controller = controller; isPlayerInRange = true; }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player out of range");
        uiCanvas.SetActive(false);
        controller = null; isPlayerInRange = false;
    }
}
