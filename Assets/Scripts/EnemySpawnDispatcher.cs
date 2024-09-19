using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDispatcher : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private List<GameObject> enemyPrefabs;

    public void Dispatch(string enemyType, int spawner, Quaternion rotation)
    {
        GameObject enemyPrefab = enemyPrefabs.Find(match => match.GetComponent<BaseEnemy>().GetType().ToString() == enemyType);
        if (enemyPrefab == null)
        {
            Debug.Log(string.Format("Enemy type: {0} does not exist", enemyType));

            return;
        }

        BaseEnemy enemy = Instantiate(enemyPrefab, GameManager.Instance.GetSpawnerPathPoint(spawner, 0).position, rotation).GetComponent<BaseEnemy>();
        enemy.SetSpawnerNumber(spawner);
        enemy.findNextDestinationPoint();
    }


}
