using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemy : Enemy
{
    public override EnemyManager.EnemyType EnemyType()
    {
        return EnemyManager.EnemyType.Normal;
    }

    public override int GetEnemyScore()
    {
        return 3;
    }

    public override float GetSpeed()
    {
        return 0.1f;
    }

    public override int GetMaxHealth()
    {
        return 10;
    }
}
