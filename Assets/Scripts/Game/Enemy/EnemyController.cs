using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    [Header("Movement Stats")]
    [Range(5f, 30f)][SerializeField] private float moveForce = 10f;
    [Range(0f, 10f)][SerializeField] private float drag = 3f;
    [Range(1f, 10f)][SerializeField] private float minimumApproachDistance = 3f;
    [SerializeField] private float followRange = 30f;

    [Header("Enemy Behavior Settings")]
    [SerializeField] private int score;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private float blowUpTimer = 2f;

    private EnemyManager enemyManager;
    private GameManager gameManager;
    private ItemManager itemManager;
    private Transform target;
    private Transform bossMovementTarget;
    private float timePassAfterSpawn = 0f;

    public int Score { get { return score; } }

    /// <summary>
    /// Initialize controller of enemy(Set target of enemy)
    /// </summary>
    /// <param name="enemyManager"></param>
    /// <param name="gameManager"></param>
    /// <param name="itemManager"></param>
    /// <param name="bossMovementTarget"></param>
    public void Init(EnemyManager enemyManager, GameManager gameManager, ItemManager itemManager, Transform bossMovementTarget)
    {
        this.enemyManager = enemyManager;
        this.gameManager = gameManager;
        this.itemManager = itemManager;
        this.bossMovementTarget = bossMovementTarget;

        target = gameManager.Player.transform;
    }

    protected override void Start()
    {
        rigidBody.drag = drag;
    }

    protected override void Update()
    {
        base.Update();
        
        if(weaponHandler == null)
        {
            timePassAfterSpawn += Time.deltaTime;
            if (timePassAfterSpawn > blowUpTimer) Die();
        }
    }

    protected override void FixedUpdate()
    {
        if (!gameManager.IsGameActive) { rigidBody.velocity = Vector3.zero; return; }

        if (!isBoss) Movement(movementDirection);
        else
        {
            float distance = DistanceToTarget();
            if (distance > 0.1f)
                transform.position = Vector2.Lerp(transform.position, bossMovementTarget.position, Time.fixedDeltaTime * statHandler.Speed);
        }
    }

    /// <summary>
    /// Handle movement direction of enemy by type of enemy(Suicide Bomber, Close Range Enemy, Long Range Enemy, Boss)
    /// </summary>
    protected override void HandleAction()
    {
        base.HandleAction();

        if (target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;

        if (distance <= followRange)
        {
            lookAtDirection = direction;

            if (weaponHandler == null) { movementDirection = direction; return; }

            if (distance < weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target;
                RaycastHit2D hit;
                if (isBoss) { hit = Physics2D.Raycast(WeaponPivot.position, direction, weaponHandler.AttackRange * 1.5f, (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget); }
                else hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f, (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }
            }

            if (isBoss) return;

            if (weaponHandler.AttackRange > minimumApproachDistance)
            {
                if (distance > minimumApproachDistance) movementDirection = direction;
                else movementDirection = Vector3.zero;
            }
            else
            {
                if (distance > weaponHandler.AttackRange) movementDirection = direction;
                else movementDirection = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// Called when enemy dies
    /// </summary>
    public override void Die()
    {
        rigidBody.gravityScale = 1f;

        if (isBoss) itemManager.SpawnHPItem(transform.position + new Vector3(-1, 0));
        else { if (Random.Range(0, 20) % 5 == 0) itemManager.SpawnHPItem(transform.position); }
        base.Die();
        enemyManager.RemoveEnemyOnDeath(this);
    }

    /// <summary>
    /// Calculates distance from the target
    /// </summary>
    /// <returns>Returns distance from the target</returns>
    protected float DistanceToTarget()
    {
        if (isBoss) return Vector3.Distance(transform.position, bossMovementTarget.position);
        return Vector3.Distance(transform.position, target.position);
    }

    /// <summary>
    /// Calculate direction to target
    /// </summary>
    /// <returns>Returns direction to target</returns>
    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    /// <summary>
    /// Calculate movement velocity and direction of enemy
    /// </summary>
    /// <param name="direction"></param>
    private void Movement(Vector2 direction)
    {
        if (rigidBody.velocity.magnitude > statHandler.Speed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * statHandler.Speed;
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
    /// Collision Resolution of enemy
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (weaponHandler != null) return;
        if((1 << target.gameObject.layer & (1 << collision.gameObject.layer)) != 0)
        {
            ResourceController controller = Helper.GetComponent_Helper<ResourceController>(collision.gameObject);
            controller.ChangeHealth(-2);
            Die();
        }
    }
}
