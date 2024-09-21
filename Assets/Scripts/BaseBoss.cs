using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoss : BaseEnemy
{

    [Header("Boss References")]
    [SerializeField] protected float passiveEffectRange;
    [SerializeField] protected float passiveEffectCooldown;
    [SerializeField] protected LayerMask turretsMask;
    [SerializeField] protected BossPassiveType bossPassiveType;



    private float passiveEffectTimer;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Setup()
    {
        base.Setup();
        passiveEffectTimer = passiveEffectCooldown;
    }

    protected override void Update()
    {
        base.Update();
        
        passiveEffectTimer -= Time.deltaTime;

        if(passiveEffectTimer <= 0f)
        {
            passiveEffectTimer = passiveEffectCooldown;
            PassiveEffect();
        }

    }

    public override void DealDamage(DamageInfo damageInfo)
    {
        base.DealDamage(damageInfo);
    }

        public override void DestroyEnemy(bool dropGold = false)
    {
        base.DestroyEnemy(dropGold);
    }

    protected virtual void PassiveEffect()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, passiveEffectRange, (Vector2)transform.position, 0f, turretsMask);


        foreach (RaycastHit2D hit in hits)
        {
            BaseTurret turret = hit.transform.GetComponent<BaseTurret>();
            if (turret != null)
            {
                turret.ApplyBossPassiveEffect(bossPassiveType);
            }

        }
    }

}
