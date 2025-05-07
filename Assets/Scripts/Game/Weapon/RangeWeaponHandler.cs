using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] private int bulletIndex;
    [SerializeField] private float bulletSize = 1f;
    [SerializeField] private float duration;
    [SerializeField] private float spread;
    [SerializeField] private int numberOfProjectilesPerShot;
    [SerializeField] private float multipleProjectileAngle;
    [SerializeField] private Color projectileColor;
    private ProjectileManager projectileManager;

    public int BulletIndex { get { return bulletIndex; } }
    public float BulletSize { get { return bulletSize; } }
    public float Duration { get { return duration; } }
    public float Spread { get { return spread; } }
    public int NumberOfProjectilesPerShot { get { return numberOfProjectilesPerShot; } }
    public float MultipleProjectileAngle { get { return multipleProjectileAngle; } }
    public Color ProjectileColor { get { return projectileColor; } }

    protected override void Start()
    {
        base.Start();

        projectileManager = ProjectileManager.Instance;
    }

    /// <summary>
    /// Creates projectile when attack is enabled by player input or controller in enemy
    /// </summary>
    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberOfProjectilesPerShot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;

            CreateProjectile(Controller.LookAtDirection, angle);
        }
    }

    /// <summary>
    /// Create projectile
    /// </summary>
    /// <param name="lookDirection"></param>
    /// <param name="angle"></param>
    private void CreateProjectile(Vector2 lookDirection, float angle)
    {
        projectileManager.ShootBullet(this, projectileSpawnPosition.position, RotateVector2(lookDirection, angle));
    }

    /// <summary>
    /// Rotates projectile by movement direction
    /// </summary>
    /// <param name="v"></param>
    /// <param name="degree"></param>
    /// <returns></returns>
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
