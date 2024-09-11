using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class BaseTurret : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] protected float baseRange = 3f;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected DamageType damageType = DamageType.normal;
    [SerializeField] protected int cost = 100;


    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected TurretUpgrades upgrades;
    [SerializeField] protected GameObject upgradesMenu;

    private bool upgradesMenuOpen = false;

    protected virtual void Start()
    {
       range = baseRange;
       damage = baseDamage;
    }

    protected virtual void OnEnable()
    {
        upgrades = gameObject.AddComponent<TurretUpgrades>();
    }


    // Get Damage info based on current turret stats
    public DamageInfo GetDamageInfo()
    {
        return new DamageInfo(damageType, damage);
    }

    // Try upgrading a particular statistic - must pass validation
    public void UpgradeStatistic(UpgradeTypes type)
    {
        upgrades.Upgrade(type);
    }

    // Implement this to have different upgrade results per turret type
    protected abstract void HandleUpgradeEvent(Upgrade upgrade, BaseTurret turret);


    // Damage Type must be set to deal proper damage type by turret
    public void SetDamageType(DamageType type)
    {
        this.damageType = type;
    }


    // Draw range for debug
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }

    public int GetCost()
    {
        return cost;
    }

    public TurretUpgrades GetUpgrades()
    {
        return upgrades;
    }

    private void OnMouseDown()
    {
        upgradesMenuOpen = !upgradesMenuOpen;
        upgradesMenu.SetActive(upgradesMenuOpen);
    }

}
