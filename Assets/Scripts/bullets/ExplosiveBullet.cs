using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : BaseBullet
{
    protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
        enemy.DealDamage(damageInfo);
        Destroy(gameObject);
    }
}
