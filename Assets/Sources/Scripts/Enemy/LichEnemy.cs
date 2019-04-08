using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichEnemy : Enemy
{
    public override EnemyManager.EnemyType EnemyType()
    {
        return EnemyManager.EnemyType.Fast;
    }

    public override int GetEnemyScore()
    {
        return 6;
    }

    public override float GetSpeed()
    {
        return 0.2f;
    }

    public override int GetMaxHealth()
    {
        return 6;
    }
}
