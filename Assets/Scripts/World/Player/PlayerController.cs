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
    }

    protected override void Start()
    {
        ChangeCharacter(GlobalGameManager.Instance.GetCurrentCharacterData());
    }

    protected override void Update()
    {
        if (!worldManager.IsWorldActive) return;
        base.Update();
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
        movementDirection = inputValue.Get<Vector2>().normalized;
    }

    void OnLook(InputValue inputValue)
    {
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

    void OnZoom(InputValue inputValue)
    {
        if (!worldManager.IsWorldActive) return;

        float scroll = inputValue.Get<float>();
        camController.Scroll = scroll;
    }
}
