using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [Range(0.1f, 1f)][SerializeField] private float zoomRate = 1f;
    [Range(0.1f, 2f)][SerializeField] private float zoomSpeed = 1f;
    [Range(1f, 2f)][SerializeField] private float minZoom = 1f;
    [Range(4f, 5f)][SerializeField] private float maxZoom = 5f;

    private Camera cam;
    private PlayerController playerController;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float mapWidth;
    private float mapHeight;
    private float currentZoom;
    private float newZoom;
    private float scroll = 0f;

    public bool IsCameraReady { get; set; } = false;
    public float Scroll { get => scroll; set => scroll = value; }

    private void Awake()
    {
        cam = Camera.main;
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        currentZoom = cam.orthographicSize;
        newZoom = cam.orthographicSize;
    }

    private void Update()
    {
        if (!WorldManager.Instance.IsWorldActive) return;
        CalculateCamPosition_Zoom();
    }

    private void LateUpdate()
    {
        if (!WorldManager.Instance.IsWorldActive) return;
        ChangeCamPositionNZoom();
    }

    /// <summary>
    /// Set Camera Boundary using tilemap
    /// </summary>
    /// <param name="tilemap"></param>
    public void SetCameraBoundsFromTilemap(Tilemap tilemap)
    {
        Vector3 min = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 max = tilemap.CellToWorld(tilemap.cellBounds.max);

        minBounds = new Vector2(min.x, min.y);
        maxBounds = new Vector2(max.x, max.y);
        mapWidth = maxBounds.x - minBounds.x;
        mapHeight = maxBounds.y - minBounds.y;

        IsCameraReady = true;
    }

    /// <summary>
    /// Calculate Camera Position depends on boundary
    /// </summary>
    private void ChangeCamPositionNZoom()
    {
        currentZoom = Mathf.Lerp(currentZoom, newZoom, Time.deltaTime * zoomSpeed);
        cam.orthographicSize = currentZoom;

        Vector3 playerPosition = playerController.transform.position;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 currentCameraPos = transform.position;
        Vector3 cameraPos = transform.position;
        cameraPos.x = Mathf.Clamp(playerPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        cameraPos.y = Mathf.Clamp(playerPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);
        cameraPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, cameraPos, 1f);
    }

    /// <summary>
    /// Calculate orthographic size of camera output
    /// </summary>
    /// <param name="scroll"></param>
    public void CalculateCamPosition_Zoom()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float maxCamHeight = mapHeight / 2f;
        float maxCamWidth = mapWidth / 2f / cam.aspect;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            newZoom = currentZoom - Mathf.Clamp(scroll * zoomRate, -1f, 1f);
            Debug.Log(newZoom);
            newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
            scroll = 0;
        }
    }
}
