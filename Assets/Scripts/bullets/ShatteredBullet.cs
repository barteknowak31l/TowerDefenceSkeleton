using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredBullet : BaseBullet
{

	protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
        if (enemy != enemyToIgnore)
        {

			DamageInfo newDamageInfo = new DamageInfo(damageInfo.damageType, DamageSource.bulletShatter, damageInfo.amount/6);
			enemy.DealDamage(newDamageInfo);
            Debug.Log(newDamageInfo.damageSource + "||" + newDamageInfo.damageType + "||" + newDamageInfo.amount);
            Destroy(gameObject);
        }

    }

}
