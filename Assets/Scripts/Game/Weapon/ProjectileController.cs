using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangeWeaponHandler weaponHandler;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Transform pivot;
    private ProjectileManager projectileManager;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;

    public bool fxOnDestroy = true;

    /// <summary>
    /// Awake is called once when scripts are being loaded.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidBody = Helper.GetComponent_Helper<Rigidbody2D>(gameObject);
        pivot = transform.GetChild(0);
    }

    /// <summary>
    /// Initialize Projectile GameObject.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="weaponHandler"></param>
    /// <param name="projectileManager"></param>
    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;
        this.weaponHandler = weaponHandler;
        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        transform.right = this.direction;

        if (direction.x < 0) pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }

    /// <summary>
    /// Update is called every time
    /// </summary>
    private void Update()
    {
        if (!isReady) { return; }
        if (!GameManager.Instance.IsGameActive) { rigidBody.velocity = Vector3.zero; return; }

        currentDuration += Time.deltaTime;

        if (currentDuration > weaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rigidBody.velocity = direction * weaponHandler.Speed;
    }

    /// <summary>
    /// Collision Resolution of Projectile GameObject
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f, fxOnDestroy);
        }
        else if (weaponHandler.target.value == (weaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            ResourceController resourceController = Helper.GetComponent_Helper<ResourceController>(collision.gameObject);
            if (resourceController == null) return;

            if(resourceController.ChangeHealth(-weaponHandler.Power))
                DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    /// <summary>
    /// Destroy Projectile when has impact with a wall or enemy, also show sfx if createFx is true.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="createFx"></param>
    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx) projectileManager.CreateImpactParticlesAtPosition(position, weaponHandler);
        Destroy(this.gameObject);
    }
}

