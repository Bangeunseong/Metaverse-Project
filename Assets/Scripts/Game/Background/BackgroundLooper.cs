using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [Header("Background GameObjects")]
    [SerializeField] private Transform mountains;
    [SerializeField] private Transform trees;
    [SerializeField] private Transform mountainsFar;
    
    [Header("Background Moving Speed")]
    [Range(0.1f, 10f)][SerializeField] private float speed_Mountains = 2f;
    [Range(0.1f, 10f)][SerializeField] private float speed_Mountains_Far = 0.5f;
    [Range(0.1f, 10f)][SerializeField] private float speed_Trees = 4f;

    private BoxCollider2D boxCollider;
    private float halfWidth = 0f;

    private void Awake()
    {
        boxCollider = Helper.GetComponent_Helper<BoxCollider2D>(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        halfWidth = boxCollider.bounds.size.x / 2;
    }

    private void FixedUpdate()
    {
        if(mountains.position.x < -halfWidth) { mountains.position = new Vector3(0, mountains.position.y, mountains.position.z); }
        if(trees.position.x < -halfWidth) { trees.position = new Vector3(0, trees.position.y, trees.position.z); }
        if(mountainsFar.position.x < -halfWidth) { mountainsFar.position = new Vector3(0, mountainsFar.position.y, mountainsFar.position.z); }

        float mountainsX = mountains.position.x - speed_Mountains * Time.fixedDeltaTime;
        float mountainsFarX = mountainsFar.position.x - speed_Mountains_Far * Time.fixedDeltaTime;
        float treesX = trees.position.x - speed_Trees * Time.fixedDeltaTime;

        mountains.position = new Vector3(mountainsX, mountains.position.y, mountains.position.z);
        mountainsFar.position = new Vector3(mountainsFarX, mountainsFar.position.y, mountainsFar.position.z);
        trees.position = new Vector3(treesX, trees.position.y, trees.position.z);
    }
}
