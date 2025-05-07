using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private static readonly int IsAttack = Animator.StringToHash("IsAttack_trig");
    
    [Header("Attack info")]
    [SerializeField] private float delay = 1.0f;
    [SerializeField] private float weaponSize = 1f;
    [SerializeField] private float power = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private AudioClip attackClip;

    public float Delay { get => delay; set => delay = value; }
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }
    public float Power { get => power; set => power = value; }
    public float Speed { get => speed; set => speed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    protected BaseController Controller { get; private set; }
    private Animator animator;
    private SpriteRenderer weaponRenderer;

    protected virtual void Awake()
    {
        Controller = Helper.GetComponentInParent_Helper<BaseController>(gameObject, true);
        animator = Helper.GetComponentInChildren_Helper<Animator>(gameObject, true);
        weaponRenderer = Helper.GetComponentInChildren_Helper<SpriteRenderer>(gameObject, true);

        animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start() { }

    /// <summary>
    /// Called when player or enemy attacks
    /// </summary>
    public virtual void Attack()
    {
        AttackAnimation();

        if (attackClip != null) { SoundManager.PlayClip(attackClip); }
    }

    /// <summary>
    /// Plays attack animation when player or enemy attacks
    /// </summary>
    private void AttackAnimation()
    {
        animator.SetTrigger(IsAttack);
    }

    /// <summary>
    /// Rotate weapon sprite when rotation is over 180 degrees.
    /// </summary>
    /// <param name="isLeft"></param>
    public virtual void Rotate(bool isLeft)
    {
        weaponRenderer.flipY = isLeft;
    }
}
