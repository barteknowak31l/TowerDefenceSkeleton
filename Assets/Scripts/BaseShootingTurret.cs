using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public abstract class BaseShootingTurret : BaseTurret
{
    [Header("Shooting stuff")]
    [SerializeField] protected float rotationSpeed = 200f;
    [SerializeField] protected float baseFireCooldown = 1f;
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected LayerMask enemyMask;

    

    private float fireCooldown;

    protected Transform target;

    // Override this to have various shoot abilities per turret type (i.e spread shot)
    protected virtual void Shoot()
    {
        Debug.Log(string.Format("{0} SHOOT", name));
        fireCooldown = baseFireCooldown;
    }

    protected override void Start()
    {
        base.Start();
        fireCooldown = 0f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        upgrades.UpgradeSuccess += HandleUpgradeEvent;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        fireCooldown -= Time.deltaTime;

        if (!CheckIfTargetInRange())
        {
            target = null;
        }
        else
        {
            if(fireCooldown < 0f)
            {
                Shoot();
            }
        }
    }    

    // Upgrade methods below can be further overrided to have different outcomes per turret type
    protected override void HandleUpgradeEvent(Upgrade upgrade, BaseTurret turret)
    {
        if (turret != this) return;

        if (upgrade.type == UpgradeTypes.attackSpeed)
        {
            CalculateFireCooldown();
        }
        if (upgrade.type == UpgradeTypes.damage)
        {
            CalculateDamage();
        }
        if (upgrade.type == UpgradeTypes.range)
        {
            CalculateRange();
        }
    }

    protected virtual void CalculateFireCooldown()
    {
        fireCooldown = baseFireCooldown - 0.2f * upgrades.GetUpgradeByType(UpgradeTypes.attackSpeed).level;
    }

    protected virtual void CalculateDamage()
    {
        damage = baseDamage + baseDamage * upgrades.GetUpgradeByType(UpgradeTypes.damage).level + 1;
    }

    protected virtual void CalculateRange()
    {
        range = baseRange + baseRange * upgrades.GetUpgradeByType(UpgradeTypes.range).level + 1;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        rotationPoint.rotation = Quaternion.RotateTowards(rotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    private bool CheckIfTargetInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= range;
    }

}
