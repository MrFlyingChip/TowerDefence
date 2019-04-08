using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator
{
    const float TIME_BETWEEN_SPAWNS = 1.5f;
    
    GameAnimator animator;
    Transform leftBorder;
    Transform rightBorder;
    Transform tower;

    Queue<Enemy> enemiesToSpawn = new Queue<Enemy>();
    Dictionary<int, WalkController> enemies = new Dictionary<int, WalkController>();
    List<int> enemiesToDestroy = new List<int>();

    public void Init(GameAnimator gameAnimator, Transform rBorder, Transform lBorder, Transform towerTransform)
    {
        animator = gameAnimator;
        leftBorder = lBorder;
        rightBorder = rBorder;
        tower = towerTransform;
        EnemyManager.Instance.OnEnemySpawned += OnEnemySpawned;
        EnemyManager.Instance.OnWaveStarted.AddListener(OnWaveStarted);
    }

    void OnEnemySpawned(Enemy enemy)
    {
        enemiesToSpawn.Enqueue(enemy);
    }

    void OnWaveStarted()
    {
        enemies.Clear();
        enemiesToDestroy.Clear();
        animator.StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (enemiesToSpawn.Count > 0)
        {
            yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS);
            Enemy enemyToSpawn = enemiesToSpawn.Dequeue();
            Spawn(enemyToSpawn);
        }
    }

    void Spawn(Enemy enemyToSpawn)
    {
        GameObject enemyPrefab = PrefabHolder.Instance.GetEnemy(enemyToSpawn.EnemyType());
        if (enemyPrefab)
        {
            float t = Random.Range(0f, 1f);
            Vector3 pointToSpawn = Vector3.Lerp(leftBorder.position, rightBorder.position, t);
            GameObject enemy = animator.CreateObject(enemyPrefab, pointToSpawn, tower);
            WalkController walkController = enemy.GetComponent<WalkController>();
            if (!walkController)
            {
                return;
            }

            enemyToSpawn.EnemyDelegate += OnEnemyGotDamage;
            walkController.Init(tower.localPosition, enemyToSpawn.GetSpeed(), enemyToSpawn.enemyId);
            walkController.SignalOnEnemyDamagedTower.AddListener(OnEnemyWalkedToTower);
            enemies[enemyToSpawn.enemyId] = walkController;
        }
    }

    void OnEnemyWalkedToTower(int id)
    {
        enemies.Remove(id);
        EnemyManager.Instance.DamageTower(id);
    }

    public void DamageEnemies(Vector3 explosionPos)
    {
        if (enemiesToDestroy.Count == enemies.Count)
        {
            return;
        }

        foreach (int id in enemies.Keys)
        {
            if (!enemiesToDestroy.Contains(id))
            {
                Vector3 enemyPos = enemies[id].transform.localPosition;
                EnemyManager.Instance.DamageEnemy(id, explosionPos, enemyPos);
            }
        }
    }

    void OnEnemyGotDamage(Enemy enemy)
    {
        if (enemies.ContainsKey(enemy.enemyId))
        {
            if (enemy.currentHealth <= 0)
            {
                Object.Destroy(enemies[enemy.enemyId].gameObject);
                enemiesToDestroy.Add(enemy.enemyId);
            }
        }
    }

    public void HideAllEnemies()
    {
        SetActiveEnemies(false);
    }

    public void ShowAllEnemies()
    {
        SetActiveEnemies(true);
    }

    void SetActiveEnemies(bool active)
    {
        foreach (WalkController enemy in enemies.Values)
        {
            if (enemy)
            {
                enemy.gameObject.SetActive(active);
            }
        }
    }
    
    private static EnemyAnimator _instance = null;
    public static EnemyAnimator Instance => _instance ?? (_instance = new EnemyAnimator());
}
