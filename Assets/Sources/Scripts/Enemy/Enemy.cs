using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public EnemyDelegate EnemyDelegate;
    public int enemyId;
    public int currentHealth;

    public virtual EnemyManager.EnemyType EnemyType()
    {
        return EnemyManager.EnemyType.Normal;
    }

    public virtual int GetEnemyScore()
    {
        return 0;
    }

    public virtual int GetMaxHealth()
    {
        return 0;
    }

    public virtual float GetSpeed()
    {
        return 0;
    }

    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        EnemyDelegate?.Invoke(this);
    }

    public Enemy Clone()
    {
        return (Enemy) MemberwiseClone();
    }
}
