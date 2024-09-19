using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] private int waveNumber = 0;
    [SerializeField] private float spawnDelay = 1.0f;
    [SerializeField] private float currentSpawnDelay = 1.0f;
    [SerializeField] private WaveList waves;
    [SerializeField] private WaveListElement currentWave;
    [SerializeField] private bool isSpawning = false;

    [SerializeField] public int enemiesAlive = 0;

    [Header("Wave Breaks")]
    [SerializeField] private float breakDuration = 5.0f;
    [SerializeField] private float currentBreakTime = 0.0f;
    private bool isInBreak = false;


    [Header("BossWaves")]
    [SerializeField] public int boss1Wave;
    [SerializeField] public int boss2Wave;
    [SerializeField] public int boss3Wave;

    private EnemySpawnDispatcher enemySpawnDispatcher;
    public GameObject skipButton;
    public static WaveSpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    void Start()
    {

        if(!TryGetComponent<EnemySpawnDispatcher>(out enemySpawnDispatcher))
        {
            Debug.LogError(string.Format("{} has no EnemySpawnDispatcher attached!", name));
            return;
        }

        WaveConfigData config = JSONReader.Instance.Read<WaveConfigData>("wave_config");

        if (config == null)
        {
            Debug.LogError("Failed to load WaveConfigData.");
            return;
        }

        //config.PrintData();

        waves = new WaveList(config.waves ?? new Wave[0]); 

        Debug.Log(string.Format("waves count: {0}", waves.waves.Count));
        currentSpawnDelay = spawnDelay;
        StartWave();
    }

    void Update()
    {
    
        if (isInBreak)
        {
            HandleWaveBreak();
            return;
        }
     
        currentSpawnDelay -= Time.deltaTime;
        if (!isSpawning && currentWave.enemies.Count <= 0 && enemiesAlive <= 0)
        {
            StartWaveBreak();

        }
        if (currentSpawnDelay < 0 && isSpawning)
        {
            currentSpawnDelay = spawnDelay;
            SpawnRandomEnemy();
        }


     

    }


   public void SkipWaveBreak()
    {
        currentBreakTime = 0;

    }

    void StartWave()
    {
        if (waveNumber < waves.waves.Count)
        {
            currentSpawnDelay = spawnDelay;
            currentWave = waves.waves[waveNumber];
            isSpawning = true;
            Debug.Log(string.Format("STARTING WAVE: {0}", currentWave.name));
        }
        else
        {
            // TODO create win event and handle win logic somewhere (GameManger probably)
            Debug.Log("YOU WIN!");

        }
    }

    void SpawnRandomEnemy()
    {
        WaveEnemy waveEnemy = currentWave.GetRandomEnemyAndDecreaseAmount();
      
        if (currentWave.enemies.Count <= 0)
        {
            isSpawning = false;
        }


        foreach (int spawner in currentWave.spawners)
        {
            Debug.Log(string.Format("Spawning enemy type: {0}, amount left to spawn: {1}", waveEnemy.enemyType, waveEnemy.amount));
            enemySpawnDispatcher.Dispatch(waveEnemy.enemyType, spawner, Quaternion.identity);
        }

    }
    void StartWaveBreak()
    {
        isInBreak = true; 
        currentBreakTime = breakDuration;
        if (waveNumber == boss1Wave)
        {
            Camera.main.transform.GetComponent<CameraZoomer>().ChangeCameraSize(1);
        }
        else if (waveNumber == boss2Wave)
        {
            Camera.main.transform.GetComponent<CameraZoomer>().ChangeCameraSize(2);
        }


    }

    void HandleWaveBreak()
    {
        skipButton.SetActive(true);

        currentBreakTime -= Time.deltaTime;

        if (currentBreakTime <= 0)
        {
            skipButton.SetActive(false);

            isInBreak = false;
            waveNumber++; 
            StartWave();
        }
    }
}
