using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] protected float maxHp = 10;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float movementSpeed = 1;
    [SerializeField] protected float baseMovementSpeed = 1;
    [SerializeField] protected int goldDropped = 50;

    [Header("Effects")]
     protected bool isFrozen = false;
    [SerializeField] protected float freezeDuration = 2f;
     protected float freezeTimer = 0f;
    [SerializeField] protected float shatterChance = 0.2f;
    [SerializeField] protected int freezeStacks = 0;
    [SerializeField] protected int maxFreezeStacks = 5; 
    [SerializeField] protected float freezeSlowPercentage = 0.2f;
     protected float timeSinceLastFreezeDamage = 0f;
    [SerializeField] protected float freezeResetTime = 3.0f;
    protected bool isRecentlyFrozen = false;
     protected float recentlyFrozenDuration = 3.0f;
    protected float recentlyFrozenTimer = 0f;
     protected bool isIgnited = false;
     protected int igniteStacks = 0;
    [SerializeField] protected int maxIgniteStacks = 10;
    [SerializeField] protected float igniteDamage = 2f;
    [SerializeField] protected float igniteInterval = 1f;
    [SerializeField]  protected float igniteResetTime = 3.0f;
    protected float timeSinceLastFireDamage = 0f;

    [Header("Resistance")]
    [SerializeField] protected float fireResistance = 0.0f; 
    [SerializeField] protected float iceResistance = 0.0f;
    private Coroutine igniteCoroutine;

    [Header("Navigation/Movement")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected int currentPathStage = -1;
    [SerializeField] protected Vector2 movementDirection;
    [SerializeField] protected bool isOnLastStage = false;
    [SerializeField] protected Transform nextPathStage;
    [SerializeField] protected int spawnerNumber;



	protected virtual void Start()
    {
        baseMovementSpeed=movementSpeed;
        rigid = GetComponent<Rigidbody2D>();
        Setup();
        //findNextDestinationPoint();
    }

    protected virtual void Setup()
    {
        currentHp = maxHp;
        movementSpeed = baseMovementSpeed;
        WaveSpawner.Instance.enemiesAlive++;
    }

    protected virtual void Update()
    {
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                freezeStacks = 0;

                movementSpeed = baseMovementSpeed;
                isFrozen = false; 
                isRecentlyFrozen = true; 
                timeSinceLastFreezeDamage = 0;
                recentlyFrozenTimer =0;
            }
        }
        else
        {
            timeSinceLastFreezeDamage += Time.deltaTime;
            if (isRecentlyFrozen)
            {
                recentlyFrozenTimer += Time.deltaTime;
                if (recentlyFrozenTimer >= recentlyFrozenDuration)
                {
                    isRecentlyFrozen = false; 
                }
            }
            if (timeSinceLastFreezeDamage >= freezeResetTime && freezeStacks > 0)
            {
                freezeStacks = 0;

                movementSpeed = baseMovementSpeed; 
               
            }
            Vector2 distanceToNextStage = new Vector2(transform.position.x - nextPathStage.position.x,
                transform.position.y - nextPathStage.position.y);

            if (!isOnLastStage && distanceToNextStage.magnitude < 0.1f)
            {
                findNextDestinationPoint();
            }
            else if (isOnLastStage && distanceToNextStage.magnitude < 0.1f)
            {
                DestroyEnemy();
            }
			Vector2 normalizedDirection = distanceToNextStage.normalized;
			float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
			Move();

		}
    }
    protected virtual void DestroyEnemy(bool dropGold = false)
    {
        Debug.Log("enemy zniszczony");
        if (dropGold)
        {
            GameManager.Instance.AddGold(goldDropped);
        }
        WaveSpawner.Instance.enemiesAlive--;
        Destroy(gameObject);
    }


	/* TODO separate methods to take fire/electric/ice damage ??? - some part of this mechanic (more related to bullets) might be
     implemented in BaseBullet.OnEnemyContact() or its derivatives */

	public virtual void DealDamage(DamageInfo damageInfo)
    {
        try
        {

            switch (damageInfo.damageType)
            {
                case DamageType.normal: DealNormalDamage(damageInfo); break;
                case DamageType.fire: DealFireDamage(damageInfo); break;
                case DamageType.ice: DealIceDamage(damageInfo); break;
                default: Debug.Log(string.Format("Unrecognized Damage Type: {}",damageInfo.damageType.ToString())); break;
            }

        }
        catch { }


        if (currentHp <= 0)
        {
            DestroyEnemy(true);
        }
    }

    protected virtual void DealNormalDamage(DamageInfo damageInfo)
    {
        currentHp -= damageInfo.amount;


    }

    protected virtual void DealIceDamage(DamageInfo damageInfo)
    {
  

        switch (damageInfo.damageSource)
        {
            case DamageSource.singleTurret:
                ShatterChance(shatterChance);
                DealStandardIceDamage(damageInfo);  
                break;
            case DamageSource.sniperTurret:
                DealStandardIceDamage(damageInfo) ;
                break;
            case DamageSource.bulletShatter:
            DealStandardIceDamage(damageInfo) ;
            break;
            case DamageSource.cannonTurret:
                DealStandardIceDamage(damageInfo) ; 
                break;
            case DamageSource.auraTurret:
                AddFreezeStack();
                break;
            default: Debug.Log(string.Format("Unrecognized Damage Source: {}", damageInfo.damageSource.ToString())); break;
        }
    }

    protected virtual void DealStandardIceDamage(DamageInfo damageInfo)
    {
        timeSinceLastFreezeDamage = 0;
        float damageAfterResistance = damageInfo.amount * (1.0f - iceResistance);
        currentHp -= damageAfterResistance;

        AddFreezeStack();
    }

 

    protected virtual void DealFireDamage(DamageInfo damageInfo)
    {
        float damageAfterResistance = damageInfo.amount * (1.0f - fireResistance);

        currentHp -= damageAfterResistance;

        switch (damageInfo.damageSource)
        {
            case DamageSource.singleTurret:
                DealFireDamageSingleTurret(damageInfo);
                break;
            case DamageSource.sniperTurret:
                AddMultipleIgniteStacks(4) ; 
                break;
             case DamageSource.bulletShatter:
             AddIgniteStack();
             break;
            case DamageSource.cannonTurret:
                AddIgniteStack() ; 
                break;
            case DamageSource.auraTurret:
                AddIgniteStack();
                break;
            default: Debug.Log(string.Format("Unrecognized Damage Source: {}", damageInfo.damageSource.ToString())); break;
        }
    }
    protected virtual void DealFireDamageSingleTurret(DamageInfo damageInfo)
    {


        AddIgniteStack();
        if (igniteStacks >= 2)
        {
            DealImmediateIgniteDamage();
        }
    }


    protected void ShatterChance(float shatterChance)
    {
        if (UnityEngine.Random.value < shatterChance && isFrozen)
        {
            DestroyEnemy(true);
        }

    }  
    
    protected void AddFreezeStack()
    {
        if (isFrozen || isRecentlyFrozen)
        {
            if (freezeStacks < maxFreezeStacks)
            {
                freezeStacks++;
                movementSpeed = baseMovementSpeed * (1 - freezeStacks * freezeSlowPercentage);
            }
        }
        else
        {
            if (freezeStacks+1 >= maxFreezeStacks)
            {
                Freeze();
            }
            else
            {
                freezeStacks++;
                movementSpeed = baseMovementSpeed * (1 - freezeStacks * freezeSlowPercentage);
                timeSinceLastFreezeDamage = 0;
            }
        }

    }


    protected void Freeze()
    {
        isFrozen = true;
        freezeTimer = freezeDuration;
        rigid.velocity = Vector2.zero;
        movementSpeed = 0;

    }
    protected void AddMultipleIgniteStacks(int stackCount)
    {
        for (int i = 0; i < stackCount; i++)
        {
            if (igniteStacks < maxIgniteStacks)
            {
                timeSinceLastFireDamage = 0;

                igniteStacks++;
               
            }
        }
        DealImmediateIgniteDamage();

    }
    protected void AddIgniteStack()
    {
        if (igniteStacks < maxIgniteStacks)
        {
            timeSinceLastFireDamage = 0;

            igniteStacks++;
            if (!isIgnited)
            {
                igniteCoroutine = StartCoroutine(IgniteDamageOverTime());
            }
        }
    }


    protected void DealImmediateIgniteDamage()
    {
        float totalDamage = igniteDamage * igniteStacks *  igniteResetTime*(1.0f - fireResistance);
        currentHp -= totalDamage;

        if (currentHp <= 0)
        {
            DestroyEnemy(true);
        }
        igniteStacks = 0;
        isIgnited = false;
    }
    protected IEnumerator IgniteDamageOverTime()
    {
        isIgnited = true;

        while (igniteStacks > 0)
        {
            yield return new WaitForSeconds(igniteInterval);

            float damageAfterResistance = igniteDamage * igniteStacks * (1.0f - fireResistance);
            currentHp -= damageAfterResistance;
            if (currentHp <= 0)
            {
                DestroyEnemy(true);
                yield break;
            }


            timeSinceLastFireDamage += igniteInterval;

            if (timeSinceLastFireDamage >= igniteResetTime)
            {
                timeSinceLastFireDamage = 0;
                    igniteStacks -=1;
            }
        }

        isIgnited = false;
        //igniteStacks = 0;
    }

    public void findNextDestinationPoint()
    {
        // find current and next points from GameManager
        // calculate direction vector between them
        // update movementDirection vector on this 

        Transform currentPoint;

        if (currentPathStage == -1)
        {
            currentPoint = GameManager.Instance.GetSpawnerPathPoint(spawnerNumber, 0);
            nextPathStage = GameManager.Instance.GetSpawnerPathPoint(spawnerNumber, currentPathStage + 1);

        }
        else
        {
            currentPoint = nextPathStage;
            if (!isOnLastStage)
            {
                nextPathStage = GameManager.Instance.GetSpawnerPathPoint(spawnerNumber, currentPathStage + 1);

            }
        }

        movementDirection = new Vector2(nextPathStage.position.x - currentPoint.position.x,
            nextPathStage.position.y - currentPoint.position.y).normalized;
    
        currentPathStage++;
        if (currentPathStage == GameManager.Instance.enemyPathPoints.Length - 1)
            isOnLastStage = true;
    }
    
    protected void Move()
    {
        rigid.velocity = movementDirection * movementSpeed;
    }

    public void SetSpawnerNumber(int number)
    {
        spawnerNumber = number;
    }


    //TEMPORARY - delete after Piotr base hp system
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Base")
        {
            DestroyEnemy();
        }
    }
}
