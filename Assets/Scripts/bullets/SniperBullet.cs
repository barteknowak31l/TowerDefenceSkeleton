using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : BaseBullet
{
	[Header("Points")]
    [SerializeField]private Transform firingPoint;
	[Header("ShatteredShot")]
	[SerializeField]private GameObject bulletPrefab;
	[SerializeField]private float fanAngle = 20f;
	[SerializeField]private int projectileCount = 5;
	[SerializeField]private float projectileSpeed = 5f;

	protected override void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo)
    {
<<<<<<< HEAD
		enemy.
        enemy.DealDamage(damageInfo);
		ShootFan();
		Destroy(gameObject);
    }
	public void ShootFan()
=======
        enemy.DealDamage(damageInfo);

		GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();
		bullet.SetDamageInfo(damageInfo);
		ShootFan(bulletObj);
		Destroy(gameObject);
    }
	public void ShootFan(GameObject bulletObj)
>>>>>>> origin/Create_Towerv2
	{
		float halfAngle = fanAngle / 2f;

		for (int i = 0; i < projectileCount; i++)
		{
<<<<<<< HEAD
			float angle = Mathf.Lerp(-halfAngle, halfAngle, (float)i / (projectileCount - 1));
			float angleRad = angle * Mathf.Deg2Rad;

			Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

			direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * direction;

			GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();
			bullet.SetDamageInfo(damageInfo);

=======
			// Oblicz proporcjonalny k¹t dla ka¿dego pocisku
			float angle = Mathf.Lerp(-halfAngle, halfAngle, (float)i / (projectileCount - 1));
			float angleRad = angle * Mathf.Deg2Rad;

			// Oblicz kierunek na podstawie k¹ta
			Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

			// Obróæ kierunek zgodnie z obrotem obiektu strzelaj¹cego
			direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * direction;

			// Utwórz pocisk


			// Ustaw kierunek pocisku
>>>>>>> origin/Create_Towerv2
			Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
			rb.velocity = direction * projectileSpeed;
		}
	}
}
