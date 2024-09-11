using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Color baseColor;
    public Color hoverColor;


    private GameObject turretObj;
    public BaseTurret turret;

    private void Start()
    {
        spriteRenderer.color = baseColor;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = baseColor;
    }

    private void OnMouseDown()
    {
        if (turretObj != null) return;

        GameObject turretToSpawn = BuildManager.Instance.GetSelectedTower();
        
        if (turretToSpawn == null) return;

        turretObj = Instantiate(turretToSpawn, transform.position, Quaternion.identity);
        turret = turretObj.GetComponent<BaseTurret>();
        if(!GameManager.Instance.SpendGold(turret.GetCost()))
        {
            Destroy(turretObj);
        }

    }
}
