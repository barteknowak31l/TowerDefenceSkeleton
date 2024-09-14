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
        
        if (turret == null) {

            spriteRenderer.color = hoverColor;
            if (previewTurret == null)
            {
                GameObject turretToPreview = BuildManager.Instance.GetSelectedTower();

                if (turretToPreview != null)
                {
                    previewTurret = Instantiate(turretToPreview, transform.position, Quaternion.identity);
                    SetTurretPreviewMode(previewTurret);
                }
            }


        }

     


    }

    private void OnMouseExit()
    {
        spriteRenderer.color = baseColor;

        if (previewTurret != null)
        {
            Destroy(previewTurret);
        }
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

    private void SetTurretPreviewMode(GameObject turret)
    {
        foreach (var renderer in turret.GetComponentsInChildren<Renderer>())
        {
            Color color = renderer.material.color;
            renderer.material.color = color;
        }

        NormalShootingTurret shootingScript = turret.GetComponent<NormalShootingTurret>();
        if (shootingScript != null)
        {
            shootingScript.enabled = false;
        }
        BoxCollider2D turretCollider = turret.GetComponent<BoxCollider2D>();
        if (turretCollider != null)
        {
            turretCollider.enabled = false;
        }


    }
}
