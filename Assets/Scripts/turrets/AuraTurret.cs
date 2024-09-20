using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraTurret : BaseAuraTurret
{
    protected override void OnEnable()
    {
        base.OnEnable();
        upgrades.UpgradeSuccess += HandleUpgradeEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        upgrades.UpgradeSuccess -= HandleUpgradeEvent;
    }

    protected override void Start()
	{
		base.Start();
	}
	protected override void Update()
	{
		base.Update();
	}

    protected override void HandleUpgradeEvent(BaseTurret turret)
    {
        base.HandleUpgradeEvent(turret);

    }
}
