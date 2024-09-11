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
    

    private void OnEnable()
    {
        electricButton.interactable = false;
        fireButton.interactable = false;
        iceButton.interactable = false;
        CheckIfDmgTypeButtonsShouldBeEnabled();
    }

    public void BuyUpgrade(int upgradeType)
    {
        turret.UpgradeStatistic((UpgradeTypes)upgradeType);
        CheckIfDmgTypeButtonsShouldBeEnabled();
    }

    private void CheckIfDmgTypeButtonsShouldBeEnabled()
    {
        if (turret.GetUpgrades() != null && turret.GetUpgrades().favouredUpgrade != null && turret.GetDamageInfo().damageType == DamageType.normal)
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
