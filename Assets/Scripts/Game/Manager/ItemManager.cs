using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject[] recoverItemPrefabs;
    [SerializeField] private ParticleSystem impactParticleSystem;

    private GameManager gameManager;

    // Singleton
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
                instance = Helper.GetComponent_Helper<ItemManager>(GameObject.FindWithTag(nameof(ItemManager)));
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void SpawnHPItem(Vector2 startPos)
    {
        int randomIndex = Random.Range(0, recoverItemPrefabs.Count());

        GameObject origin = recoverItemPrefabs[randomIndex];
        GameObject obj = Instantiate(origin, startPos, Quaternion.identity);

        ItemController controller = obj.GetComponent<ItemController>();
        controller.Init(gameManager, this);
    }

    public void CreateImpactParticlesAtPosition(Vector3 position)
    {
        impactParticleSystem.transform.position = position;
        ParticleSystem.EmissionModule em = impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Random.Range(1, 5)));

        ParticleSystem.MainModule mainModule = impactParticleSystem.main;
        mainModule.startSpeedMultiplier = 2f;
        impactParticleSystem.Play();
    }
}
