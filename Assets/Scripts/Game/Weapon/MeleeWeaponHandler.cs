using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : WeaponHandler
{
    [Header("Melee Attack Data")]
    public Vector2 collideBoxSize = Vector2.one;

    protected override void Start()
    {
        base.Start();
        collideBoxSize *= WeaponSize;
    }

    /// <summary>
    /// Called when player or enemy attacks
    /// </summary>
    public override void Attack()
    {
        base.Attack();

        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)Controller.LookAtDirection * collideBoxSize.x, collideBoxSize, 0, Vector2.zero, 0, target);
        if (hit.collider == null) return;

        ResourceController resourceController = Helper.GetComponent_Helper<ResourceController>(hit.collider.gameObject);
        if (resourceController == null) return;

        resourceController.ChangeHealth(-Power);
    }

    /// <summary>
    /// Rotate when player's or enemy's lookAt direction changed
    /// </summary>
    /// <param name="isLeft"></param>
    public override void Rotate(bool isLeft)
    {
        // base.Rotate(isLeft);

        if (isLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
