using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class BaseTurret : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] protected float baseRange;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected float baseFireCooldown;
    [SerializeField] protected float fireCooldown;
    [SerializeField] protected DamageType damageType = DamageType.normal;
    [SerializeField] protected int cost = 100;
    [SerializeField] public string upgradesConfigFile;



    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected TurretUpgrades upgrades;
    [SerializeField] protected GameObject upgradesMenu;
    private RectTransform upgradesMenuRectTransform;
    private Camera mainCamera;
    private bool upgradesMenuOpen = false;
    Vector3 currentPosition;
    private static BaseTurret currentlyOpenMenu = null;

    Vector3 basePosition;
    protected virtual void Start()
    {
  
        upgradesMenuRectTransform = upgradesMenu.GetComponent<RectTransform>();
        mainCamera = Camera.main;

        CalculateFireCooldown();
        CalculateDamage();
        CalculateRange();

        currentPosition = upgradesMenuRectTransform.position;
        basePosition = currentPosition;
    }



    protected virtual void OnEnable()
    {
        upgrades = gameObject.AddComponent<TurretUpgrades>();
    }


    protected virtual void CalculateFireCooldown()
    {
        baseFireCooldown = upgrades.GetUpgradeByType(UpgradeTypes.attackSpeed).value;
        fireCooldown = baseFireCooldown;
    }

    protected virtual void CalculateDamage()
    {
        baseDamage = upgrades.GetUpgradeByType(UpgradeTypes.damage).value;
        damage = baseDamage;


    }

    protected virtual void CalculateRange()
    {
        baseRange = upgrades.GetUpgradeByType(UpgradeTypes.range).value;
        range = baseRange;

    }

    // Get Damage info based on current turret stats
    public DamageInfo GetDamageInfo()
    {
        return new DamageInfo(damageType, damage);
    }

    // Upgrade turret tier
    public void UpgradeTurret()
    {
        upgrades.UpgradeTurret();
    }

    // Implement this to have different upgrade results per turret type
    protected abstract void HandleUpgradeEvent(BaseTurret turret);


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
        if (currentlyOpenMenu != null && currentlyOpenMenu != this)
        {
            currentlyOpenMenu.CloseUpgradesMenu();
        }

        upgradesMenuOpen = !upgradesMenuOpen;

        if (upgradesMenuOpen)
        {
            ShowUpgradesMenu();
            currentlyOpenMenu = this;
        }
        else
        {
            CloseUpgradesMenu();
        }
    }


    private void ShowUpgradesMenu()
    {
        upgradesMenu.SetActive(true);
        currentPosition = basePosition;
        currentPosition.z -=15;

        if (upgradesMenuRectTransform.position.x > 0 && upgradesMenuRectTransform.position.y > 0)
        {
            currentPosition.x -= 2;
            currentPosition.y -= 2;
            upgradesMenuRectTransform.position = currentPosition;
        }
        else if (upgradesMenuRectTransform.position.x < 0 && upgradesMenuRectTransform.position.y > 0)
        {
            currentPosition.y -= 2;
            upgradesMenuRectTransform.position = currentPosition;
        }
        else if (upgradesMenuRectTransform.position.x > 0 && upgradesMenuRectTransform.position.y < 0)
        {
            currentPosition.x -= 2;
            upgradesMenuRectTransform.position = currentPosition;
        }
        else
        {
            upgradesMenuRectTransform.position = currentPosition;

        }
    }

    private void CloseUpgradesMenu()
    {
        upgradesMenu.SetActive(false);
        upgradesMenuOpen = false;

        if (currentlyOpenMenu == this)
        {
            currentlyOpenMenu = null;
        }
    }

    public int GetTurretLevel()
    {
        return upgrades.turretLevel;
    }

}
