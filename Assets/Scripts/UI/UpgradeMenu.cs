using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private BaseTurret selectedTurret; 

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button fireButton;
    [SerializeField] private Button iceButton;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text turretLevelText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text turretFireDescriptionText;
    [SerializeField] private TMP_Text turretIceDescriptionText;
    public void SetTurret(BaseTurret turret)
    {
        selectedTurret = turret;

        CheckIfDmgTypeButtonsShouldBeEnabled();
        UpdateUpgradeCost();
        UpdateTurretLevel();
        UpdateTurretStats();
        UpdateTurretDescription();

        if (selectedTurret != null && selectedTurret.GetDamageInfo().damageType == DamageType.ice)
        {

            iceButton.gameObject.SetActive(true);
            fireButton.gameObject.SetActive(false);


        }
        else if (selectedTurret != null && selectedTurret.GetDamageInfo().damageType == DamageType.fire)
        {
            fireButton.gameObject.SetActive(true);
            iceButton.gameObject.SetActive(false);


        }
    }

    public void BuyUpgrade()
    {

        if (selectedTurret == null || selectedTurret.GetTurretLevel() == 5)
        {
            upgradeCostText.text = "Max Level";

            return;

        }

        selectedTurret.UpgradeTurret();
        UpdateUpgradeCost();
        UpdateTurretLevel();
        UpdateTurretStats();
        UpdateTurretDescription();
        if (selectedTurret.GetDamageInfo().damageType == DamageType.normal)
        CheckIfDmgTypeButtonsShouldBeEnabled();


    }

    private bool CheckIfMaxLvl()
    {
        return selectedTurret.GetTurretLevel() >= GameManager.Instance.maxUpgradeLvl;
    }

    private void CheckIfDmgTypeButtonsShouldBeEnabled()
    {
        if (selectedTurret != null && selectedTurret.GetUpgrades() != null &&
        selectedTurret.GetTurretLevel() >= GameManager.Instance.damageTypeSelectionLevel &&
        selectedTurret.GetDamageInfo().damageType == DamageType.normal)
        {
         
            fireButton.gameObject.SetActive(true);
            iceButton.gameObject.SetActive(true);
        }
        else
        {
            fireButton.gameObject.SetActive(false);
            iceButton.gameObject.SetActive(false);
        }
    }

    public void SelectDamageType(int type)
    {
        if (selectedTurret.GetDamageInfo().damageType != DamageType.normal) return;

    
        if (selectedTurret != null && selectedTurret.GetDamageInfo().damageType == DamageType.normal)
        {
            selectedTurret.SetDamageType((DamageType)type);
            if (type == (int)DamageType.fire)
            {
                iceButton.gameObject.SetActive(false);
            }
            else if (type == (int)DamageType.ice)
            {
                fireButton.gameObject.SetActive(false);

            }
        }
    }

    private void UpdateUpgradeCost()
    {
        if (selectedTurret != null)
        {
            TurretUpgrades turretUpgrades = selectedTurret.GetComponent<TurretUpgrades>();
            int upgradeCost = turretUpgrades.cost;
            upgradeCostText.text = upgradeCost.ToString();
        }
        if (selectedTurret.GetTurretLevel() == 5)
        {
            upgradeCostText.text = "Max Level";


        }
    }
    private void UpdateTurretLevel()
    {
        if (selectedTurret != null)
        {
            turretLevelText.text = "Level: " + selectedTurret.GetTurretLevel();
        }
    }
    private void UpdateTurretStats()
    {
        if (selectedTurret != null)
        {
            TurretUpgrades turretUpgrades = selectedTurret.GetComponent<TurretUpgrades>();

            Upgrade damageUpgrade = turretUpgrades.GetUpgradeByType(UpgradeTypes.damage);
            Upgrade attackSpeedUpgrade = turretUpgrades.GetUpgradeByType(UpgradeTypes.attackSpeed);
            Upgrade rangeUpgrade = turretUpgrades.GetUpgradeByType(UpgradeTypes.range);

            if (damageUpgrade != null)
                damageText.text = damageUpgrade.value.ToString();

            if (attackSpeedUpgrade != null)
                attackSpeedText.text = attackSpeedUpgrade.value.ToString();

            if (rangeUpgrade != null)
                rangeText.text =  rangeUpgrade.value.ToString();
        }
    }
    private void UpdateTurretDescription()
    {
        if (selectedTurret == null) return;

        string descriptionFire = "";
        string descriptionIce = "";



        switch (selectedTurret.GetDamageInfo().damageSource)
        {
            case DamageSource.singleTurret:
                    descriptionFire += "Applies ignite stacks that deals dmg over time, deals instant dmg at 2 stacks and removes all stacks";
                    descriptionIce += "Applies chill stacks that slows enemy. After 5 stacks enemy is freezed - " +
                    " freezed enemies have a slight chance to shatter";
                break;
            case DamageSource.cannonTurret:
                    descriptionFire += "Applies AOE ignite stacks that deals dmg over time";
                descriptionIce += "Applies AOE chill stacks that slows enemy.";
                break;
            case DamageSource.auraTurret:
                    descriptionFire += "Applies AOE ignite stacks that deals dmg over time";
                descriptionIce += "Applies AOE chill stacks that slows enemy.";
                break;
            case DamageSource.sniperTurret:
                descriptionFire += "Applies ignite stacks that deals dmg over time, deals instant dmg at 4 stacks and removes all stacks, shattered bullets applies only 1 stack";
                descriptionIce += "Applies chill stacks that slows enemy. Applies to shattered bullets too";
                break;
        }

        turretFireDescriptionText.text = descriptionFire;
        turretIceDescriptionText.text = descriptionIce;
    }
}
