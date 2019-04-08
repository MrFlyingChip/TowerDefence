using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyDelegate(Enemy enemy);

public class EnemyManager
{
    public SignalVoid OnWaveEnded = new SignalVoid();
    public SignalVoid OnWaveStarted = new SignalVoid();
    public EnemyDelegate OnEnemySpawned;
    
    const int INCREASE_SCORE_PER_WAVE = 10;

    private int currentWaveScore;

    private List<Enemy> registeredEnemies = new List<Enemy>();
    private Stack<Enemy> enemiesToSpawn = new Stack<Enemy>();
    private Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

    private bool waveEnded = true;
    private int enemyId = 0;
    
    public enum EnemyType
    {
        Fast,
        Strong, 
        Normal
    }

    public void Init()
    {
        registeredEnemies.Clear();
        currentWaveScore = 0;
        waveEnded = true;
        enemyId = 0;
        RegisterEnemies();
    }

    void RegisterEnemies()
    {
        registeredEnemies.Add(new GruntEnemy());
        registeredEnemies.Add(new LichEnemy());
        registeredEnemies.Add(new GolemEnemy());
    }

    public void StartWave()
    {
        if (!waveEnded)
        {
            return;
        }

        waveEnded = false;
        enemies.Clear();
        enemiesToSpawn.Clear();
        enemyId = 0;
        currentWaveScore += INCREASE_SCORE_PER_WAVE;
        ConsiderEnemiesToSpawn();
        
        OnWaveStarted.Invoke();
    }

    void ConsiderEnemiesToSpawn()
    {
        int waveScore = 0;
        List<Enemy> enemiesCanBeSpawned = new List<Enemy>(registeredEnemies);
        while (waveScore <= currentWaveScore)
        {
            int index = Random.Range(0, enemiesCanBeSpawned.Count);
            Enemy enemyToSpawn = enemiesCanBeSpawned[index].Clone();
            if (enemyToSpawn.GetEnemyScore() + waveScore <= currentWaveScore)
            {
                enemiesToSpawn.Push(enemyToSpawn);
                waveScore += enemyToSpawn.GetEnemyScore();
            }
            else
            {
                enemiesCanBeSpawned.RemoveAt(index);
            }
            
            if (enemiesCanBeSpawned.Count == 0)
            {
                break;
            }
        }

        while (enemiesToSpawn.Count > 0)
        {
            Enemy enemyToSpawn = enemiesToSpawn.Pop();
            SpawnEnemy(enemyToSpawn);
        }
}

    void SpawnEnemy(Enemy enemy)
    {
        enemy.enemyId = ++enemyId;
        enemy.currentHealth = enemy.GetMaxHealth();
        enemy.EnemyDelegate += OnEnemyGotDamage;
        enemies[enemy.enemyId] = enemy;
        OnEnemySpawned?.Invoke(enemy);
    }

    void EndWave()
    {
        waveEnded = true;
        OnWaveEnded.Invoke();
    }

    void OnEnemyGotDamage(Enemy enemy)
    {
        if (enemies.ContainsKey(enemy.enemyId))
        {
            if (enemy.currentHealth <= 0)
            {
                TowerManager.Instance.GetGold(enemy.GetEnemyScore());
                TowerManager.Instance.GetScore(enemy.GetMaxHealth());
                DeleteEnemy(enemy);
            }
        }
    }

    void DeleteEnemy(Enemy enemy)
    {
        enemy.EnemyDelegate -= OnEnemyGotDamage;
        enemies.Remove(enemy.enemyId);

        if (enemies.Count == 0)
        {
            EndWave();
        }
    }

    public void DamageTower(int id)
    {
        if (enemies.ContainsKey(id))
        {
            Enemy enemy = enemies[id];
            TowerManager.Instance.GetDamage(enemy.GetMaxHealth());
            DeleteEnemy(enemy);
        }
    }

    public void DamageEnemy(int id, Vector3 explosionPos, Vector3 enemyPos)
    {
        if (!enemies.ContainsKey(id))
        {
            return;
        }

        float distance = Vector3.Distance(explosionPos, enemyPos);
        if (distance > TowerManager.DAMAGE_RADIUS)
        {
            return;
        }

        float damageToDeal = Mathf.Lerp(TowerManager.MAX_TOWER_DAMAGE, TowerManager.MIN_TOWER_DAMAGE, distance / TowerManager.DAMAGE_RADIUS);
        enemies[id].GetDamage(Mathf.CeilToInt(damageToDeal));
    }

    private static EnemyManager _instance = null;
    public static EnemyManager Instance => _instance ?? (_instance = new EnemyManager());
}
