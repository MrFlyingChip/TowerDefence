using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemy : Enemy
{
    public override EnemyManager.EnemyType EnemyType()
    {
        return EnemyManager.EnemyType.Strong;
    }

    public override int GetEnemyScore()
    {
        return 10;
    }

    public override float GetSpeed()
    {
        return 0.06f;
    }

    public override int GetMaxHealth()
    {
        return 20;
    }
}
