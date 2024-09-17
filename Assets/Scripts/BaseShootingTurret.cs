using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public abstract class BaseShootingTurret : BaseTurret
{
    [Header("Shooting stuff")]
    [SerializeField] protected float rotationSpeed = 200f;
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected LayerMask enemyMask;

    protected Transform target;

    // Override this to have various shoot abilities per turret type (i.e spread shot)
    protected virtual void Shoot()
    {
        Debug.Log(string.Format("{0} SHOOT", name));
        fireCooldownTimer = fireCooldown;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        upgrades.UpgradeSuccess += HandleUpgradeEvent;
    }

    protected override void Update()
    {
        base.Update();

        if (isStunned) { return; }

        if(fireCooldownTimer > -1f)
            fireCooldownTimer -= Time.deltaTime;

        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckIfTargetInRange())
        {
            target = null;
        }
        else
        {
            if(fireCooldownTimer < 0f)
            {
                Shoot();
            }
        }
    }    

    // Upgrade methods below can be further overrided to have different outcomes per turret type
    protected override void HandleUpgradeEvent(BaseTurret turret)
    {
        if (turret != this) return;

            CalculateFireCooldown();
            CalculateDamage();
            CalculateRange();
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
