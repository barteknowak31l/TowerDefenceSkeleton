using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoss : BaseBoss
{

    [SerializeField] public float rageSpeedMultiplier;

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

    public override void DealDamage(DamageInfo damageInfo)
    {
        base.DealDamage(damageInfo);
        PassiveEffect();
    }

    protected override void PassiveEffect()
    {
        float speedFactor = 1 + ((maxHp - currentHp) / maxHp) * rageSpeedMultiplier;
        movementSpeed = baseMovementSpeed * speedFactor;
    }

}
