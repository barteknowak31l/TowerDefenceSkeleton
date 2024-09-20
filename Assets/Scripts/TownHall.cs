using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : MonoBehaviour
{
    [Header("Town Hall Stats")]
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int currentHp;
    [SerializeField] private int normalEnemyDamage =1;
    [SerializeField] private int bossDamage =3;

    [SerializeField] private GameObject gameOverMenu;

    void Start()
    {
        currentHp = maxHp;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseBoss baseBoss = collision.gameObject.GetComponent<BaseBoss>();

        if (baseBoss != null)
        {

            TakeDamage(bossDamage);

            Destroy(baseBoss.gameObject);
            return;

        }


        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            TakeDamage(normalEnemyDamage);

            baseEnemy.DestroyEnemy();

        }


    }

    private void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameManager.Instance.StopGame();


        gameOverMenu.SetActive(true);

    }
}
