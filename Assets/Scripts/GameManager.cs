using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [Header("Player Data")]
    public int gold;

    [Header("Enemy Path Points")]
    [SerializeField] public Transform enemySpawnPoint;
    [SerializeField] public Transform[] enemyPathPoints;

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
        

}
