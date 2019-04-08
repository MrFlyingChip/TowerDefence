using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyPrefab
{
    public string EnemyType;
    public GameObject GameObject;
}

public class PrefabHolder : MonoBehaviour
{
    private static PrefabHolder _instance = null;
    public static PrefabHolder Instance => _instance;
    
    private void Awake()
    {
        _instance = this;
    }

    public GameObject Bullet;
    public List<EnemyPrefab> EnemyPrefabs;
    public GameObject ExplosionPrafab;

    public GameObject GetEnemy(EnemyManager.EnemyType type)
    {
        foreach (EnemyPrefab enemyPrefab in EnemyPrefabs)
        {
            if (enemyPrefab.EnemyType == type.ToString())
            {
                return enemyPrefab.GameObject;
            }
        }

        return null;
    }

}
