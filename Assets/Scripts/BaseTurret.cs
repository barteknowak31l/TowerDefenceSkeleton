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
    protected float fireCooldownTimer;
    [SerializeField] protected DamageType damageType = DamageType.normal;
    [SerializeField] protected DamageSource damageSource;
    [SerializeField] protected int cost = 100;
    [SerializeField] public string upgradesConfigFile;

    [Header("Boss Passives")]
    [SerializeField] protected float bossPassiveRetentionTime;
    private float bossPassiveRetentionTimer;

    [Header("IceBoss Passive")]
    [SerializeField] protected bool iceBossPassiveIsUp;
    [SerializeField] protected float iceBossSlowPenaltyMultiplier;
    
    [Header("FireBoss Passive")]
    [SerializeField] protected bool fireBossPassiveIsUp;

    [Header("ElectricBoss Passive")]
    [SerializeField] protected bool electricBossPassiveIsUp;
    [SerializeField] protected bool isStunned;
    [SerializeField] protected float stunDuration;
    [SerializeField] protected float gracePeriod;
    [SerializeField] protected float gracePeriodTimer;

    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected TurretUpgrades upgrades;
    [SerializeField] protected GameObject upgradesMenu;
    //[SerializeField] private RectTransform upgradesMenuRectTransform;
    private Camera mainCamera;
    private bool upgradesMenuOpen = false;
    private static BaseTurret currentlyOpenMenu = null;
    private bool menuPositionChanged = false;

    [Header("Upgrade Sprites")]
    [SerializeField] private Sprite level3Sprite;
    [SerializeField] private Sprite level5Sprite;
    [SerializeField] private Transform turretSprite;

    protected virtual void Start()
    {
  
        //upgradesMenuRectTransform = upgradesMenu.GetComponent<RectTransform>();
        mainCamera = Camera.main;


        RecalculateStats();
        fireCooldownTimer = fireCooldown;
        bossPassiveRetentionTimer = bossPassiveRetentionTime;
        isStunned = false;
        gracePeriodTimer = gracePeriod;

    }



    protected virtual void OnEnable()
    {
        upgrades = gameObject.AddComponent<TurretUpgrades>();
    }


    protected virtual void Update()
    {

        if (bossPassiveRetentionTimer > -1f)
            bossPassiveRetentionTimer -= Time.deltaTime;



        if(bossPassiveRetentionTimer < 0f &&
            (electricBossPassiveIsUp || fireBossPassiveIsUp || iceBossPassiveIsUp)
            )
        {
            DisableBossPassives();
        }


        if (gracePeriodTimer > 0f)
        {
            gracePeriodTimer -= Time.deltaTime;
        }
        

        if (electricBossPassiveIsUp&& gracePeriodTimer <= 0f)
        {
            isStunned = true;
            StartCoroutine(removeStun());

        }

    }
    private void OnMouseDown()
    {

        GameManager.Instance.UpgradeMenu(this);
    }
    private IEnumerator removeStun()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned= false;
        gracePeriodTimer = gracePeriod;
    }

    protected void RecalculateStats()
    {
        CalculateFireCooldown();
        CalculateDamage();
        CalculateRange();
    }

    protected virtual void CalculateFireCooldown()
    {
        baseFireCooldown = upgrades.GetUpgradeByType(UpgradeTypes.attackSpeed).value;
        fireCooldown = baseFireCooldown;
        if (iceBossPassiveIsUp)
        {
            fireCooldown = fireCooldown * iceBossSlowPenaltyMultiplier;
        }


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
        return new DamageInfo(damageType,damageSource, damage);
    }

    // Upgrade turret tier
    public void UpgradeTurret()
    {
        upgrades.UpgradeTurret();
    }

    // Implement this to have different upgrade results per turret type
    protected virtual void HandleUpgradeEvent(BaseTurret turret)
    {
        Debug.Log("test upgrade");

        if (turret != this) return;

        SpriteRenderer turretSpriteRenderer = turretSprite.GetComponent<SpriteRenderer>();

   

      
        if (upgrades.turretLevel == 2)
        {

            turretSpriteRenderer.sprite = level3Sprite;
        }
        else if (upgrades.turretLevel == 5)
        {
            turretSpriteRenderer.sprite = level5Sprite;
        }
    


    }


    // Damage Type must be set to deal proper damage type by turret
    public void SetDamageType(DamageType type)
    {
        this.damageType = type;
    }


    // Draw range for debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public int GetCost()
    {
        return cost;
    }

    public TurretUpgrades GetUpgrades()
    {
        return upgrades;
    }

/*    private void OnMouseDown()
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
    }*/


    /*    private void ShowUpgradesMenu()
        {
            upgradesMenu.SetActive(true);

            if (menuPositionChanged == false)
            {
                Vector3 currentPosition = upgradesMenuRectTransform.position;

                if (upgradesMenuRectTransform.position.x > 0 && upgradesMenuRectTransform.position.y > 0)
                {
                    currentPosition.x -= 2.2f;
                    currentPosition.y -= 2.2f;
                }
                else if (upgradesMenuRectTransform.position.x < 0 && upgradesMenuRectTransform.position.y > 0)
                {
                    currentPosition.y -= 2.2f;
                }
                else if (upgradesMenuRectTransform.position.x > 0 && upgradesMenuRectTransform.position.y < 0)
                {
                    currentPosition.x -= 2.2f;
                }

                upgradesMenuRectTransform.position = currentPosition;
                menuPositionChanged = true;

            }


        }*/

    /*    private void CloseUpgradesMenu()
        {
            upgradesMenu.SetActive(false);
            upgradesMenuOpen = false;

            if (currentlyOpenMenu == this)
            {
                currentlyOpenMenu = null;
            }
        }*/

    public int GetTurretLevel()
    {
        return upgrades.turretLevel;
    }

    public virtual void ApplyBossPassiveEffect(BossPassiveType type)
    {
        switch(type)
        {
            case BossPassiveType.FireBossPassive: break;
            case BossPassiveType.IceBossPassive: ApplyIceBossPassive(); break;
            case BossPassiveType.ElectricBossPassive: ApplyElectricBossPassive(); break;
            default: Debug.Log(string.Format("Unreckognized Boss Passive Effect: {0}",type.ToString())); break; 
        }
    }

    private void DisableBossPassives()
    {
        electricBossPassiveIsUp = false;
        fireBossPassiveIsUp = false;
        iceBossPassiveIsUp = false;

        RecalculateStats();
    }

    private void ApplyIceBossPassive()
    {
        CalculateFireCooldown();
        bossPassiveRetentionTimer = bossPassiveRetentionTime;
        iceBossPassiveIsUp = true;

    }

    private void ApplyElectricBossPassive()
    {
        bossPassiveRetentionTimer = bossPassiveRetentionTime;
        electricBossPassiveIsUp = true;
    }

    private void ApplyFireBossPassive()
    {
        bossPassiveRetentionTimer = bossPassiveRetentionTime;
        fireBossPassiveIsUp = true;
    }

}
