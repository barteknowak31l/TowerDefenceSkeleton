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
    public float value; 
    public List<float> values;

    public void LevelUp()
    {
        if (level >= GameManager.Instance.maxUpgradeLvl) return;
        level++;
        CalculateNewValue();
    }

    private void CalculateNewValue()
    {
        value = values[level];
    }

    public Upgrade Type(UpgradeTypes _type)
    {
        this.type = _type;
        return this;
    }

    public Upgrade Values(List<float> _values)
    {
        this.values = _values;
        return this;
    }
    public Upgrade Value(float _value)
    {
        this.value = _value;
        return this;
    }

    public Upgrade Level(int _level)
    {
        this.level = _level;
        return this;
    }

    public Upgrade Build() { return this; }

}

public class TurretUpgrades : MonoBehaviour
{
    [Header("Upgrades")]
    public int turretLevel;
    public int cost;
    public List<int> costs;
    public List<Upgrade> upgrades;

    [Header("References")]
    [SerializeField] private BaseTurret turret;

    public delegate void UpgradeSuccessEvent(BaseTurret turret);
    public event UpgradeSuccessEvent UpgradeSuccess;
    private void Awake()
    {
        turretLevel = 0;
        turret = GetComponent<BaseTurret>();
        SetupUpgrades();

    }

    private void SetupUpgrades()
    {
        upgrades = new List<Upgrade>();



        TurretUpgradesData json = JSONReader.Instance.Read<TurretUpgradesData>(turret.upgradesConfigFile);
        costs = new List<int>(json.costs);
        CalculateTierUpgradeCost();


        List<UpgradeData> data = new List<UpgradeData>(json.upgradesData);
        UpgradeData attackSpeedData = data.Find(x => x.type == "attackSpeed");
        UpgradeData rangeData = data.Find(x => x.type == "range");
        UpgradeData damageData = data.Find(x => x.type == "damage");

        Upgrade attackSpeed = new Upgrade()
            .Type(UpgradeTypes.attackSpeed)
            .Value(attackSpeedData.values[0])
            .Values(new List<float>(attackSpeedData.values))
            .Build();

        Upgrade damage = new Upgrade()
            .Type(UpgradeTypes.damage)
            .Value(damageData.values[0])
            .Values(new List<float>(damageData.values))
            .Build();

        Upgrade range = new Upgrade()
            .Type(UpgradeTypes.range)
            .Value(rangeData.values[0])
            .Values(new List<float>(rangeData.values))
            .Build();

        upgrades.Add(attackSpeed);
        upgrades.Add(damage);
        upgrades.Add(range);
    }

    private void CalculateTierUpgradeCost()
    {
        cost = costs[turretLevel];
    }

    private void UpgradeAll()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.LevelUp();
        }
    }

    public Upgrade GetUpgradeByType(UpgradeTypes type)
    {
        return upgrades.Find(x => x.type == type);
    }
    
    public void UpgradeTurret()
    {

        // TODO store upgrade cost somewhere in JSON as INT
        if (GameManager.Instance.SpendGold(100))
        {
            turretLevel++;
            UpgradeAll();
            CalculateTierUpgradeCost();
            if (UpgradeSuccess != null)
            {
                UpgradeSuccess(turret);
            }
        }
    }
}
