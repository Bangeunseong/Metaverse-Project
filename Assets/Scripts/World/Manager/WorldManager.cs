using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    // Fields
    [SerializeField] private List<InteractHandler> handlers = new();
    [SerializeField] private GameObject cameraBoundary;
    [SerializeField] private AudioClip worldMusic;

    private WorldUIManager uiManager;
    private Camera cam;
    private CameraController camController;
    private Tilemap currentCameraBoundary;
    
    // Properties
    public bool IsWorldActive { get; private set; } = false;
    public PlayerController PlayerController { get; private set; }

    // Singleton
    private static WorldManager instance;
    public static WorldManager Instance
    { 
        get { 
            if (instance == null) 
                instance = Helper.GetComponent_Helper<WorldManager>(GameObject.FindWithTag(nameof(WorldManager))); 
            return instance; 
        } 
    }

    /// <summary>
    /// Awake is called once when scripts being loaded.
    /// </summary>
    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        cam = Camera.main;
        camController = Helper.GetComponent_Helper<CameraController>(cam.gameObject);
        currentCameraBoundary = Helper.GetComponent_Helper<Tilemap>(cameraBoundary);
        
        PlayerController = FindFirstObjectByType<PlayerController>();
        PlayerController.Init(this);
        
        uiManager = FindFirstObjectByType<WorldUIManager>();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        PlayerController.transform.position = GlobalGameManager.Instance.WorldSpawnPosition;
        camController.SetCameraBoundsFromTilemap(currentCameraBoundary);
        SoundManager.Instance.ChangeBackgroundMusic(worldMusic);

        if (!GlobalGameManager.Instance.IsFirstLoadingInWorld) { StartWorld(); }
        else GlobalGameManager.Instance.IsFirstLoadingInWorld = false;
    }

    /// <summary>
    /// Go to base world
    /// </summary>
    public void StartWorld() { IsWorldActive = true;  uiManager.GoToWorld();  }

    /// <summary>
    /// Add Event handler of specific tiles
    /// </summary>
    /// <param name="handler"></param>
    public void AddHandler(InteractHandler handler) { handlers.Add(handler); }
}
