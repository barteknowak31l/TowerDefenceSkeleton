using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [Header("Player Data")]
    [SerializeField] public int gold;
    [SerializeField] public int damageTypeSelectionLevel = 2;
    [SerializeField] public int maxUpgradeLvl = 5;

    [Header("Enemy Path Points")]
    [SerializeField] public Transform enemySpawnPoint;
    [SerializeField] public Transform[] enemyPathPoints;

    [Header("Spawners")]
    [SerializeField] public List<SpawnerData> spawners;
    public GameObject upgradeMenu;
    private BaseTurret currentlySelectedTurret;


    public TextMeshProUGUI hpText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

    }

    public bool SpendGold(int amount)
    {
        if(amount <= gold)
        {
            gold -= amount;
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public Transform GetSpawnerPathPoint(int spawner, int pathPoint)
    {
        return spawners[spawner].transforms[pathPoint];
    }

    public void StopGame()
    {
                Time.timeScale = 0;

    }
    public void UpgradeMenu(BaseTurret turret)
    {
        if (currentlySelectedTurret == turret)
        {
            CloseUpgradeMenu();
        }
        else
        {
            OpenUpgradeMenu(turret);
        }
    }

    public void OpenUpgradeMenu(BaseTurret turret)
    {
        upgradeMenu.SetActive(true);

        UpgradeMenu menu = upgradeMenu.GetComponent<UpgradeMenu>();
        menu.SetTurret(turret);


        currentlySelectedTurret = turret; 
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        currentlySelectedTurret = null; 
    }


    public void SetHpText(string hp)
    {
        hpText.text = hp;
    }

}
