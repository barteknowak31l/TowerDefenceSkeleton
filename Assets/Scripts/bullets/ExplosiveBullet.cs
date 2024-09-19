using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : BaseBullet
{
    [Header("Explosive Stats")]
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private LayerMask enemyLayer;
    protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        DamageInfo newDamageInfo = new DamageInfo(damageInfo.damageType, DamageSource.bulletShatter, damageInfo.amount);

        foreach (var hitEnemy in hitEnemys)
        {
            BaseEnemy baseEnemy = hitEnemy.gameObject.GetComponent<BaseEnemy>();
            if (baseEnemy != null)
			{
				baseEnemy.DealDamage(newDamageInfo);
			}
        }
        Destroy(gameObject);
    }
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
