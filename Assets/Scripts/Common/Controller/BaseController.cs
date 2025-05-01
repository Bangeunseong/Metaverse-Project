using UnityEngine;

[RequireComponent(typeof(AnimationHandler))]
[RequireComponent(typeof(StatHandler))]
[RequireComponent(typeof(Rigidbody2D))]

public class BaseController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Rigidbody2D rigidBody;
    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    protected Vector2 movementDirection = Vector2.zero;
    protected Vector2 lookAtDirection = Vector2.zero;
    protected bool isInteractable = false;

    // Properties
    public Vector2 MovementDirection { get { return movementDirection; } }
    public Vector2 LookAtDirection { get { return lookAtDirection; } }
    public bool IsInteractable { get => isInteractable; set => isInteractable = value; }

    protected virtual void Awake()
    {
        rigidBody = Helper.GetComponent_Helper<Rigidbody2D>(gameObject);
        animationHandler = Helper.GetComponent_Helper<AnimationHandler>(gameObject);
        statHandler = Helper.GetComponent_Helper<StatHandler>(gameObject);
    }

    protected virtual void Start() { }

    protected virtual void Update() 
    {
        HandleAction();
        Rotate(lookAtDirection);
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
    }

    protected virtual void HandleAction() { }

    private void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed;

        rigidBody.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = isLeft;
    }
}
