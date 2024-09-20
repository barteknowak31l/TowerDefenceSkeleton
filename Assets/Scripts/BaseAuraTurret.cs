using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseAuraTurret : BaseTurret
{
	[Header("Layers")]
	[SerializeField] private LayerMask enemyLayer;

    protected override void HandleUpgradeEvent(BaseTurret turret)
    {
        base.HandleUpgradeEvent(turret);

        if (turret != this) return;

        CalculateFireCooldown();
        CalculateDamage();
        CalculateRange();
    }
    protected override void Start()
	{
		base.Start();
		StartCoroutine(UpdateEnemiesInRange());
		Debug.Log("Started");
	}

	protected IEnumerator UpdateEnemiesInRange()
	{
		while (true)
		{
			Debug.Log("Checking for enemies");
			Shoot();
			yield return new WaitForSeconds(base.fireCooldown);
		}
	}
	protected void Shoot()
	{
		Debug.Log("Shoot");
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
		foreach (var enemy in enemies)
		{
			BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
			if(baseEnemy != null)
			{
				Debug.Log("Enemy in range");
				baseEnemy.DealDamage(new DamageInfo(damageType, damageSource, damage));
			}
		}
	}
}
