using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BaseTurret turret;
    [SerializeField] private Button electricButton;
    [SerializeField] private Button fireButton;
    [SerializeField] private Button iceButton;
    [SerializeField] private Button upgradeButton;

    private void OnEnable()
    {
        electricButton.interactable = false;
        fireButton.interactable = false;
        iceButton.interactable = false;
        CheckIfDmgTypeButtonsShouldBeEnabled();
    }

    public void BuyUpgrade()
    {
        turret.UpgradeTurret();
        CheckIfDmgTypeButtonsShouldBeEnabled();
        if(CheckIfMaxLvl())
        {
            upgradeButton.interactable = false;
        }
    }

    private bool CheckIfMaxLvl()
    {
        return turret.GetTurretLevel() >= GameManager.Instance.maxUpgradeLvl;
    }

    private void CheckIfDmgTypeButtonsShouldBeEnabled()
    {
        if (turret.GetUpgrades() != null && turret.GetTurretLevel() >= GameManager.Instance.damageTypeSelectionLevel  && turret.GetDamageInfo().damageType == DamageType.normal)
        {
            electricButton.interactable = true;
            fireButton.interactable = true;
            iceButton.interactable = true;
        }
    }

    public void SelectDamageType(int type)
    {
        if(turret.GetDamageInfo().damageType == DamageType.normal)
        {
            turret.SetDamageType((DamageType)type);
            electricButton.interactable = false;
            fireButton.interactable = false;
            iceButton.interactable = false;
        }
    }

}
