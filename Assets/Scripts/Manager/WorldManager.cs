using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private static WorldManager instance;
    [SerializeField] public static List<InteractHandler> handlers = new List<InteractHandler>();

    public PlayerController playerController { get; private set; }
    public static WorldManager Instance 
    { 
        get { 
            if (instance == null) 
                instance = Helper.GetComponent_Helper<WorldManager>(GameObject.FindWithTag(nameof(WorldManager))); 
            return instance; 
        } 
    }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        playerController = FindFirstObjectByType<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController.Init(this);
        playerController.transform.position = GameManager.worldSpawnPosition;
    }
}
