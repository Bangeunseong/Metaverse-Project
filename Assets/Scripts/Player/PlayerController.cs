using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    [Range(1f, 5f)][SerializeField] private float zoomSpeed = 5f;
    [Range(10f, 20f)][SerializeField] private float changeSpeed = 20f;
    [Range(2f, 5f)][SerializeField] private float minZoom = 5f;
    [Range(6f, 10f)][SerializeField] private float maxZoom = 10f;

    private Camera cam;
    private CinemachineVirtualCamera vCam;
    private WorldManager worldManager;
    
    public void Init(WorldManager worldManager)
    {
        this.worldManager = worldManager;
        cam = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
        vCam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera as CinemachineVirtualCamera;
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
        isInteractable = inputValue.Get<float>() > 0f;
    }

    void OnFire()
    {

    }

    void OnZoom(InputValue inputValue)
    {
        float scroll = inputValue.Get<float>();
        if (scroll != 0f)
        {
            float currentZoom = vCam.m_Lens.OrthographicSize;
            float newZoom = currentZoom - scroll * zoomSpeed;
            newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
            vCam.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, newZoom, Time.deltaTime * changeSpeed);
        }
    }
}
