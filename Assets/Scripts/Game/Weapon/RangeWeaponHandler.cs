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

    public int BulletIndex => bulletIndex;
    public float BulletSize => bulletSize;
    public float Duration => duration;
    public float Spread => spread;
    public int NumberOfProjectilesPerShot => numberOfProjectilesPerShot;
    public float MultipleProjectileAngle => multipleProjectileAngle;
    public Color ProjectileColor => projectileColor;

    protected override void Start()
    {
        base.Start();

        projectileManager = ProjectileManager.Instance;
    }

    /// <summary>
    /// Creates projectile when attack is enabled by player input or controller in an enemy
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
