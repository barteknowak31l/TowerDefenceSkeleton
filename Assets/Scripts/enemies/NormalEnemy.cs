using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : BaseEnemy
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Setup()
    {
        base.Setup();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    protected override void DestroyEnemy(bool dropGold = false)
    {
        base.DestroyEnemy(dropGold);
    }

    public override void DealDamage(DamageInfo damageInfo)
    {
        base.DealDamage(damageInfo);
    }
}
