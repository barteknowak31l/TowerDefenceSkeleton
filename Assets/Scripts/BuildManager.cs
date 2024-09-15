using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private Sprite[] towerSprites;
    private int selectedTower = 0;

    public static BuildManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            selectedTower = -1;
        }
    }

    public GameObject GetSelectedTower()
    {
        if(selectedTower != -1)
        return towerPrefabs[selectedTower];

        return null;
    }
    public Sprite GetSelectedTowerSprite()
    {
        if (selectedTower != -1)
            return towerSprites[selectedTower];

        return null;
    }
    public void SetSelectedTower(int towerNumber)
    {
        selectedTower = towerNumber;
    }
}
