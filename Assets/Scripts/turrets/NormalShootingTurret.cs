using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShootingTurret : BaseShootingTurret
{

    protected override void Shoot()
    {
        base.Shoot();
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();

        DamageInfo damageInfo = GetDamageInfo();
        bullet.SetDamageInfo(damageInfo);
        bullet.FollowTarget(target);

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();   
    }
}
