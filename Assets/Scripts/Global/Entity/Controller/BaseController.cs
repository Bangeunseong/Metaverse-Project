using UnityEngine;

[RequireComponent(typeof(AnimationHandler))]
[RequireComponent(typeof(StatHandler))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ResourceController))]

public class BaseController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private WeaponHandler weaponPrefab;
    protected CharacterData characterData;

    protected SpriteRenderer characterRenderer;
    protected Rigidbody2D rigidBody;
    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;
    protected WeaponHandler weaponHandler;

    protected Vector2 movementDirection = Vector2.zero;
    protected Vector2 lookAtDirection = Vector2.zero;
    protected bool isInteractable = false;
    protected bool isAttacking = false;
    private float timeSinceLastAttack = float.MaxValue;

    // Properties
    public Vector2 MovementDirection { get { return movementDirection; } }
    public Vector2 LookAtDirection { get { return lookAtDirection; } }
    public Transform WeaponPivot { get { return weaponPivot; } }
    public bool IsInteractable { get => isInteractable; set => isInteractable = value; }

    protected virtual void Awake()
    {
        rigidBody = Helper.GetComponent_Helper<Rigidbody2D>(gameObject);
        animationHandler = Helper.GetComponent_Helper<AnimationHandler>(gameObject);
        statHandler = Helper.GetComponent_Helper<StatHandler>(gameObject);
        characterRenderer = Helper.GetComponentInChildren_Helper<SpriteRenderer>(gameObject);

        weaponHandler = weaponPrefab != null ? Instantiate(weaponPrefab, weaponPivot) : null;
    }

    protected virtual void Start() { }

    protected virtual void Update() 
    {
        HandleAction();
        Rotate(lookAtDirection);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
    }

    protected virtual void HandleAction() { }

    private void Movement(Vector2 direction)
    {
        direction *= statHandler.Speed;

        rigidBody.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = isLeft;

        if (weaponPivot != null) { weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ); }

        if (weaponHandler != null) weaponHandler.Rotate(isLeft);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null) return;
        if (timeSinceLastAttack <= weaponHandler.Delay) { timeSinceLastAttack += Time.deltaTime; }
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay) { timeSinceLastAttack = 0; Attack(); }
    }

    protected virtual void Attack()
    {
        if (lookAtDirection != Vector2.zero) { if(weaponHandler != null) weaponHandler.Attack(); }
    }

    public virtual void Die()
    {
        rigidBody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }
        Destroy(gameObject, 2f);
    }
}
