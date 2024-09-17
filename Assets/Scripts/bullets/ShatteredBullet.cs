using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredBullet : BaseBullet
{
<<<<<<< HEAD
    
    protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
        if (enemy.isExcluded == false)
        {
           enemy.DealDamage(damageInfo);
            Destroy(gameObject);
        }
=======
    protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
        enemy.DealDamage(damageInfo);
        Destroy(gameObject);
>>>>>>> origin/Create_Towerv2
    }
}
