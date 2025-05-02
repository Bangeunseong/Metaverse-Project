using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera cam;
    private CameraController camController;
    private WorldManager worldManager;
    
    public void Init(WorldManager worldManager)
    {
        this.worldManager = worldManager;
        cam = Camera.main;
        camController = Helper.GetComponent_Helper<CameraController>(cam.gameObject);
        ChangeCharacter(GlobalGameManager.Instance.GetCurrentCharacterData());
    }

    public void ChangeCharacter(CharacterData characterData)
    {
        this.characterData = characterData;
        characterRenderer.sprite = characterData.characterSprite;
        animationHandler.ChangeAnimationClips(characterData);
    }

    protected override void HandleAction() { }

    void OnMove(InputValue inputValue)
    {
        if (!worldManager.IsWorldActive) return;

        movementDirection = inputValue.Get<Vector2>().normalized;
    }

    void OnLook(InputValue inputValue)
    {
        if (!worldManager.IsWorldActive) return;

        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = cam.ScreenToWorldPoint(mousePosition);
        
        lookAtDirection = (worldPos - (Vector2)transform.position);
        
        if (lookAtDirection.magnitude < .9f) { lookAtDirection = Vector2.zero; }
        else { lookAtDirection = lookAtDirection.normalized; }
    }

    void OnInteract(InputValue inputValue)
    {
        if (!worldManager.IsWorldActive) return;

        isInteractable = inputValue.Get<float>() > 0f;
    }

    void OnFire()
    {

    }

    void OnZoom(InputValue inputValue)
    {
        if (!worldManager.IsWorldActive) return;

        float scroll = inputValue.Get<float>();
        camController.Scroll = scroll;
    }
}
