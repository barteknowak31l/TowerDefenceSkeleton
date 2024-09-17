using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJsonReadable { };

[System.Serializable]
public class WaveEnemy
{
    public int amount;
    public string enemyType;
}

[System.Serializable]
public class Wave
{
    public string name;
    public int[] spawners;
    public WaveEnemy[] enemies;

    public void PrintData()
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("Wave name is null or empty.");
        }
        else
        {
            Debug.Log($"Wave name: {name}");
        }

        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found in this wave.");
        }
        else
        {
            foreach (var enemy in enemies)
            {
                Debug.Log($"Enemy Type: {enemy.enemyType}, Amount: {enemy.amount}");
            }
        }
    }
}

[System.Serializable]
public class WaveConfigData : IJsonReadable
{
    public Wave[] waves;

    public void PrintData()
    {
        if (waves == null || waves.Length == 0)
        {
            Debug.LogError("Waves data is null or empty.");
            return;
        }

        foreach (var wave in waves)
        {
            wave.PrintData();
        }
    }
}


public class WaveListElement
{
    public List<WaveEnemy> enemies { get; private set; }
    public string name { get; set; }

    public List<int> spawners { get; private set; }

    public void SetEnemiesList(WaveEnemy[] enemies)
    {
        this.enemies = new List<WaveEnemy>(enemies);
    }

    public void SetSpawnersList(int[] spawners)
    {
        this.spawners = new List<int>(spawners);
    }

    public WaveEnemy GetRandomEnemyAndDecreaseAmount()
    {
        int maxRand = enemies.Count;
        int rand = Random.Range(0, maxRand);
        WaveEnemy enemy = enemies[rand];
        enemy.amount -= 1;
        if(enemy.amount == 0)
            enemies.Remove(enemy);


        return enemy;
    }


}

[System.Serializable]
public class WaveList
{
    public List<WaveListElement> waves { get; private set; }

    public WaveList(Wave[] waves)
    {
        SetWaveList(waves);
    }

    public void SetWaveList(Wave[] newWaves)
    {
        waves = new List<WaveListElement>();
        foreach (Wave wave in newWaves)
        {
            WaveListElement newElement = new WaveListElement();
            newElement.name = wave.name;
            newElement.SetEnemiesList(wave.enemies);
            newElement.SetSpawnersList(wave.spawners);
            waves.Add(newElement);
        }
    }

}