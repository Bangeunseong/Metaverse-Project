using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Range(1f, 5f)][SerializeField] private float duration = 1f;
    [Range(0f, 5f)][SerializeField] private float speed = 2f;
    [Range(0.1f, 2f)][SerializeField] private float moveForce = 1f;
    [Range(0f, 3f)][SerializeField] private float drag = 1f;
    [Range(1, 5)][SerializeField]private int recoverAmount = 1;
    [SerializeField] private LayerMask levelCollisionLayer;
    
    private Rigidbody2D rigidBody;
    private Transform target;
    private ItemManager itemManager;
    private GameManager gameManager;

    private float currentDuration;
    private Vector2 direction;

    public bool fxOnDestroy = true;

    /// <summary>
    /// Awake is called once when scripts is being loaded.
    /// </summary>
    private void Awake()
    {
        rigidBody = Helper.GetComponent_Helper<Rigidbody2D>(gameObject);
    }

    /// <summary>
    /// Initialize Projectile GameObject.
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="itemManager"></param>
    public void Init(GameManager gameManager, ItemManager itemManager)
    {
        this.itemManager = itemManager;
        this.gameManager = gameManager;
        target = gameManager.Player.transform;

        currentDuration = 0;
    }

    /// <summary>
    /// Update is called every frames per sec
    /// </summary>
    private void Update()
    {
        if (!gameManager.IsGameActive) { return; }

        currentDuration += Time.deltaTime;

        if (currentDuration > duration)
        {
            DestroyProjectile(transform.position, false);
        }
    }

    /// <summary>
    /// FixedUpdate is called every fixed frames per sec
    /// </summary>
    private void FixedUpdate()
    {
        if (!gameManager.IsGameActive) { rigidBody.velocity = Vector3.zero; return; }

        Movement(DirectionToTarget());
    }

    /// <summary>
    /// Determine movement of hp item
    /// </summary>
    /// <param name="direction"></param>
    private void Movement(Vector2 direction)
    {
        if (rigidBody.velocity.magnitude > speed)
        {
            rigidBody.velocity *= (speed / rigidBody.velocity.magnitude);
        }

        if (direction.magnitude > 0)
        {
            rigidBody.AddForce(direction.normalized * moveForce, ForceMode2D.Force);
            float dot = Vector3.Dot(Vector3.right, direction);

            rigidBody.drag = dot >= 0 ? drag : 0;
        }
        else rigidBody.drag = drag;
    }

    /// <summary>
    /// Calculate direction to target
    /// </summary>
    /// <returns>Returns direction to target in Vector2 type</returns>
    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    /// <summary>
    /// Collision Resolution of Projectile GameObject
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if ((levelCollisionLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f, fxOnDestroy);
        }
        else if ((1 << collision.gameObject.layer & (1 << target.gameObject.layer)) != 0)
        {
            Debug.Log(collision.gameObject.name);
            ResourceController resourceController = Helper.GetComponent_Helper<ResourceController>(collision.gameObject);
            
            if (resourceController == null) return;
            if (resourceController.ChangeHealth(recoverAmount))
                DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    /// <summary>
    /// Destroy Projectile when has impact with wall or enemy, also show sfx if createFx is true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="createFx"></param>
    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx) itemManager.CreateImpactParticlesAtPosition(position);
        Destroy(gameObject);
    }
}
