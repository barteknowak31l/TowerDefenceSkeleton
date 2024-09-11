using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public enum UpgradeTypes
{
    attackSpeed,
    damage,
    range,
    none
}
[System.Serializable]
public class Upgrade
{
    public UpgradeTypes type;
    public int level;
    public int baseCost;
    public int cost;
    public int costMult;
    public bool isFavoured;

    public Upgrade()
    {
        this.isFavoured = false;
    }

    public void LevelUp()
    {
        level++;
        CalculateNewCost();
    }

    private void CalculateNewCost()
    {
        baseCost = baseCost + level * costMult;
    }

    public Upgrade Type(UpgradeTypes _type)
    {
        this.type = _type;
        return this;
    }

    public Upgrade Level(int _level)
    {
        this.level = _level;
        return this;
    }

    public Upgrade BaseCost(int _baseCost)
    {
        this.baseCost = _baseCost;
        this.cost = _baseCost;
        return this;
    }
    public Upgrade CostMult(int _costMult)
    {
        this.costMult = _costMult;
        return this;
    }

    public Upgrade Build() { return this; }

}

public class TurretUpgrades : MonoBehaviour
{
    [Header("Upgrades")]
    public int turretLevel;
    public List<Upgrade> upgrades;
    public Upgrade favouredUpgrade;

    public delegate void UpgradeSuccessEvent(Upgrade upgrade, BaseTurret turret);
    public event UpgradeSuccessEvent UpgradeSuccess;

    [Header("References")]
    [SerializeField] private BaseTurret turret;

    public void Start()
    {
        turretLevel = 0;

        turret = GetComponent<BaseTurret>();

        SetupUpgrades();
    }

    private void SetupUpgrades()
    {
        favouredUpgrade = new Upgrade()
            .Type(UpgradeTypes.none)
            .Build();

        upgrades = new List<Upgrade>();

        Upgrade attackSpeed = new Upgrade()
            .Type(UpgradeTypes.attackSpeed)
            .BaseCost(200)
            .CostMult(200)
            .Build();
        Upgrade damage = new Upgrade()
            .Type(UpgradeTypes.damage)
            .BaseCost(200)
            .CostMult(200)
            .Build();
        Upgrade range = new Upgrade()
            .Type(UpgradeTypes.range)
            .BaseCost(200)
            .CostMult(200)
            .Build();

        upgrades.Add(attackSpeed);
        upgrades.Add(damage);
        upgrades.Add(range);
    }

    public void Upgrade(UpgradeTypes type)
    {
        // check cost
        // if +2lvl check if favoured
        // calc new cost and set it
        // upgrade

        Upgrade upgrade = upgrades.Find(x => x.type == type);

        Debug.Log(favouredUpgrade == null);

        if(GameManager.Instance.SpendGold(upgrade.cost))
        {
            if (upgrade.level >= 1 && favouredUpgrade.type == UpgradeTypes.none)
            {
                favouredUpgrade = upgrade;
                UpgradeTurret(upgrade);

            }
            else if (upgrade.level >= 1 && favouredUpgrade.type == upgrade.type && turretLevel < 5)
            {
                UpgradeTurret(upgrade);

            }

            if (upgrade.level == 0)
            {
                UpgradeTurret(upgrade);
            }

        }

    }

    public Upgrade GetUpgradeByType(UpgradeTypes type)
    {
        return upgrades.Find(x => x.type == type);
    }
    
    private void UpgradeTurret(Upgrade upgrade)
    {
        turretLevel++;
        upgrade.LevelUp();

        if(UpgradeSuccess != null)
        {
            UpgradeSuccess(upgrade, turret);
        }
    }
}
