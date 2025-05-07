using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> bossPrefabs;
    [SerializeField] private List<Rect> spawnAreas;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private Transform bossMovementTarget;
    [SerializeField] private Color gizmoColor = new(1, 0, 0, .3f);
    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private float timeBetweenBossWave = 3f;

    private GameManager gameManager;
    private ItemManager itemManager;
    private readonly List<EnemyController> activeEnemies = new();
    private bool enemySpawnComplete;

    public void Init(GameManager gameManager, ItemManager itemManager)
    {
        this.gameManager = gameManager;
        this.itemManager = itemManager;
    }

    public void StartWave(int waveCount)
    {
        if (waveCount <= 0) { gameManager.EndOfWave(); return; }

        if (waveRoutine != null) StopCoroutine(waveRoutine);
        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplete = false;
        if(waveCount % 5 == 0) { 
            yield return new WaitForSeconds(timeBetweenBossWave + timeBetweenSpawns); 
            SpawnBossEnemy(waveCount); 
        }
        else yield return new WaitForSeconds(timeBetweenWaves);

        int spawnCount = 3 + waveCount / 5 * 2;
        for (int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomEnemy();
        }

        enemySpawnComplete = true;
    }

    private void SpawnRandomEnemy()
    {
        if (!enemyPrefabs.Any() || !spawnAreas.Any()) { Debug.LogWarning("Enemy Prefabs or Spawn Areas are missing or not set up!"); return; }

        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        Vector2 randomPosition = new(Random.Range(randomArea.xMin, randomArea.xMax),
                                     Random.Range(randomArea.yMin, randomArea.yMax));

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        EnemyController enemyController = Helper.GetComponent_Helper<EnemyController>(spawnEnemy);
        enemyController.Init(this, gameManager, itemManager, bossMovementTarget);

        activeEnemies.Add(enemyController);
    }

    private void SpawnBossEnemy(int waveCount)
    {
        if(!bossPrefabs.Any() || bossSpawnPoint == null) { Debug.LogWarning("Boss Prefabs or Spawn point are missing or not set up!"); return; }

        GameObject randomPrefab = bossPrefabs[Mathf.Clamp(waveCount / 5 - 1, 0, bossPrefabs.Count - 1)];

        GameObject spawnBoss = Instantiate(randomPrefab, bossSpawnPoint.position, Quaternion.identity);
        EnemyController enemyController = Helper.GetComponent_Helper<EnemyController>(spawnBoss);
        enemyController.Init(this, gameManager, itemManager, bossMovementTarget);

        activeEnemies.Add(enemyController);
    }

    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        activeEnemies.Remove(enemy);
        gameManager.UpdateScore(enemy.Score);
        if (enemySpawnComplete && !activeEnemies.Any()) gameManager.EndOfWave();
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            Vector3 center = new(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new(area.width, area.height);

            Gizmos.DrawCube(center, size);
        }
    }
}
