using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove_b");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage_b");

    private Animator animator;

    private void Awake()
    {
        animator = Helper.GetComponentInChildren_Helper<Animator>(gameObject);
    }

    public void ChangeAnimationClips(CharacterData characterData)
    {
        var overrideController = new AnimatorOverrideController(characterData.controller)
        {
            ["Idle"] = characterData.idleClip,
            ["Move"] = characterData.moveClip,
            ["Damage"] = characterData.damageClip
        };

        animator.runtimeAnimatorController = overrideController;
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > .5f);
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }
}
