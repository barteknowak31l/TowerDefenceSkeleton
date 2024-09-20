using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private BaseTurret selectedTurret; 

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button fireButton;
    [SerializeField] private Button iceButton;

    private void OnEnable()
    {
        fireButton.interactable = false;
        iceButton.interactable = false;
        if (selectedTurret != null)
        {
            CheckIfDmgTypeButtonsShouldBeEnabled();
        }
    }

    public void SetTurret(BaseTurret turret)
    {
        selectedTurret = turret;
        CheckIfDmgTypeButtonsShouldBeEnabled();
    }

    public void BuyUpgrade()
    {
        Debug.Log("test kupowanie");
        if (selectedTurret == null || selectedTurret.GetTurretLevel() == 5) return;

        selectedTurret.UpgradeTurret();
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
            fireButton.interactable = true;
            iceButton.interactable = true;
        }
    }

    public void SelectDamageType(int type)
    {
        if (selectedTurret != null && selectedTurret.GetDamageInfo().damageType == DamageType.normal)
        {
            selectedTurret.SetDamageType((DamageType)type);
            fireButton.interactable = false;
            iceButton.interactable = false;
        }
    }
}
