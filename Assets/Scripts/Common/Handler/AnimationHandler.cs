using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int isMoving = Animator.StringToHash("IsMove_b");
    private static readonly int isDamage = Animator.StringToHash("IsDamage_b");

    protected Animator animator;

    private void Awake()
    {
        animator = Helper.GetComponentInChildren_Helper<Animator>(gameObject);
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(isMoving, obj.magnitude > .5f);
    }

    public void Damage()
    {
        animator.SetBool(isDamage, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(isDamage, false);
    }
}
