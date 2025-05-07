using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    [Range(5f, 15f)][SerializeField] private float moveForce = 10f;
    [Range(0f, 10f)][SerializeField] private float drag = 3f;
    [Range(0f, 15f)][SerializeField] private float speed = 8f;
    [SerializeField] private float followRange = 30f;
    [SerializeField] private int score;
    [SerializeField] private bool isBoss = false;

    private EnemyManager enemyManager;
    private GameManager gameManager;
    private ItemManager itemManager;
    private Transform target;
    private Transform bossMovementTarget;

    public int Score { get { return score; } }

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
        statHandler.Speed = speed;
    }

    protected override void FixedUpdate()
    {
        if (!gameManager.IsGameActive) { rigidBody.velocity = Vector3.zero; return; }

        if(!isBoss) Movement(movementDirection);
        else { 
            float distance = DistanceToTarget();
            if (distance > 0.1f)
                transform.position = Vector2.Lerp(transform.position, bossMovementTarget.position, Time.fixedDeltaTime * speed);
        }
    }

    protected override void HandleAction()
    {
        base.HandleAction();

        if (weaponHandler == null || target == null)
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

            if (!isBoss) 
            {
                if (weaponHandler.AttackRange > 4f)
                {
                    if (distance > 4f) movementDirection = direction;
                    else movementDirection = Vector3.zero;
                }
                else
                {
                    if(distance > weaponHandler.AttackRange) movementDirection = direction;
                    else movementDirection = Vector3.zero;
                }
            }
        }
    }

    public override void Die()
    {
        rigidBody.gravityScale = 1f;

        if (isBoss) itemManager.SpawnHPItem(transform.position + new Vector3(-1, 0));
        else { if (Random.Range(0, 20) % 5 == 0) itemManager.SpawnHPItem(transform.position); }
        base.Die();
        enemyManager.RemoveEnemyOnDeath(this);
    }
    
    protected float DistanceToTarget()
    {
        if (isBoss) return Vector3.Distance(transform.position, bossMovementTarget.position);
        return Vector3.Distance(transform.position, target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }
    
    private void Movement(Vector2 direction)
    {
        if (rigidBody.velocity.magnitude > statHandler.Speed)
        {
            rigidBody.velocity *= (statHandler.Speed / rigidBody.velocity.magnitude);
        }

        if (direction.magnitude > 0)
        {
            rigidBody.AddForce(direction.normalized * moveForce, ForceMode2D.Force);
            float dot = Vector3.Dot(Vector3.right, direction);

            rigidBody.drag = dot >= 0 ? drag : 0;
        }
        else rigidBody.drag = drag;
    }
}
