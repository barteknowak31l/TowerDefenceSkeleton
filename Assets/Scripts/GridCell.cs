using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spriteRendererPreview;

    public Color baseColor;
    public Color hoverColor;


    private GameObject turretObj;
      private GameObject previewTurret; 
    public BaseTurret turret;
    private BoxCollider2D gridCellCollider;
    private void Start()
    {
        gridCellCollider = GetComponent<BoxCollider2D>();
        spriteRenderer.color = baseColor;
    }

    private void OnMouseEnter()
    {
        if (turret == null)
        {
            spriteRenderer.color = hoverColor;

            Sprite turretSprite = BuildManager.Instance.GetSelectedTowerSprite();

            if (turretSprite != null)
            {
                spriteRendererPreview.sprite = turretSprite;
            }
        }




    }

    private void OnMouseExit()
    {
        spriteRenderer.color = baseColor;

        spriteRendererPreview.sprite = null;

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
        else
        {
            gridCellCollider.enabled = false; 
        }

    }

}
