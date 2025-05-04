using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private ParticleSystem impactParticleSystem;

    // Singleton
    private static ProjectileManager instance;
    public static ProjectileManager Instance
    {
        get
        {
            if (instance == null)
                instance = Helper.GetComponent_Helper<ProjectileManager>(GameObject.FindWithTag(nameof(ProjectileManager)));
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }

    public void ShootBullet(RangeWeaponHandler weaponHandler, Vector2 startPos, Vector2 direction)
    {
        GameObject origin = projectilePrefabs[weaponHandler.BulletIndex];
        GameObject obj = Instantiate(origin, startPos, Quaternion.identity);

        ProjectileController projectileController = Helper.GetComponent_Helper<ProjectileController>(obj);
        projectileController.Init(direction, weaponHandler, this);
    }

    public void CreateImpactParticlesAtPosition(Vector3 position, RangeWeaponHandler weaponHandler)
    {
        impactParticleSystem.transform.position = position;
        ParticleSystem.EmissionModule em = impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));

        ParticleSystem.MainModule mainModule = impactParticleSystem.main;
        mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;
        impactParticleSystem.Play();
    }
}
