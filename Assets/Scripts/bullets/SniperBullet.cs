using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SniperBullet : BaseBullet
{
	[Header("Points")]
    [SerializeField] protected Transform firingPoint;
	[Header("ShatteredShot")]
	[SerializeField] protected GameObject bulletPrefab;
	[SerializeField] protected float fanAngle = 20f;
	[SerializeField] protected int projectileCount = 5;
	[SerializeField] protected float projectileSpeed = 5f;

	private Vector2 moveDirection;
	protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
        enemy.DealDamage(damageInfo);
		enemyToIgnore = enemy;
		ShootFan(enemyToIgnore);
		Destroy(gameObject);
    }
	public override void Start()
	{
		base.Start();
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		moveDirection = rb.velocity.normalized;
	}
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		moveDirection = rb.velocity.normalized;
	}

	protected void ShootFan(BaseEnemy hitenemy)
	{
		float halfAngle = fanAngle / 2f;

		for (int i = 0; i < projectileCount; i++)
		{
			float angle = Mathf.Lerp(-halfAngle, halfAngle, (float)i / (projectileCount - 1));

			Vector2 direction = RotateVector(moveDirection, angle);

			GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();
			bullet.SetDamageInfo(damageInfo);
			bullet.enemyToIgnore = hitenemy;

			Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
			rb.velocity = direction * projectileSpeed;
		}
	}
	private Vector2 RotateVector(Vector2 vector, float angle)
	{
		float rad = angle * Mathf.Deg2Rad;
		float cos = Mathf.Cos(rad);
		float sin = Mathf.Sin(rad);
		return new Vector2(
			vector.x * cos - vector.y * sin,
			vector.x * sin + vector.y * cos
		);
	}
}
