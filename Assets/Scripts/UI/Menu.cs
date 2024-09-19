using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private Animator animator;
    [SerializeField] public bool menuOpened = true;
    [SerializeField] private TextMeshProUGUI goldDisplayText;

    //public void OnMenuOpenCloseClick()
   // {
       // menuOpened = !menuOpened;
       // animator.SetBool("Menu", menuOpened);
  //  }

    public void OnTurretMenuClick(int turretNumber)
    {
        BuildManager.Instance.SetSelectedTower(turretNumber);
    }

    private void Update()
    {
        goldDisplayText.text = GameManager.Instance.gold.ToString();
    }

}
