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
    [SerializeField] protected int goldDropped = 50;


    [Header("Navigation/Movement")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected int currentPathStage = -1;
    [SerializeField] protected Vector2 movementDirection;
    [SerializeField] protected bool isOnLastStage = false;
    [SerializeField] protected Transform nextPathStage;


    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Setup();
        findNextDestinationPoint();
    }

    protected virtual void Setup()
    {
        currentHp = maxHp;
        WaveSpawner.Instance.enemiesAlive++;
    }

    protected virtual void Update()
    {

        Vector2 distanceToNextStage = new Vector2(transform.position.x - nextPathStage.position.x,
            transform.position.y - nextPathStage.position.y);

        if (!isOnLastStage && distanceToNextStage.magnitude < 0.1f )
        {
            findNextDestinationPoint();
        }
        else if (isOnLastStage && distanceToNextStage.magnitude < 0.1f)
        {
            DestroyEnemy();
        }

        Move();
    }
    protected virtual void DestroyEnemy(bool dropGold = false)
    {
        if (dropGold)
        {
            GameManager.Instance.AddGold(goldDropped);
        }
        WaveSpawner.Instance.enemiesAlive--;
        Destroy(gameObject);
    }


    // TODO separate methods to take fire/electric/ice damage ??? - some part of this mechanic (more related to bullets) might be
    // implemented in BaseBullet.OnEnemyContact() or its derivatives

    public virtual void DealDamage(DamageInfo damageInfo)
    {
        Debug.Log(damageInfo.damageSource);

        switch(damageInfo.damageType)
        {
            case DamageType.normal: DealNormalDamage(damageInfo); break;
            case DamageType.fire: DealFireDamage(damageInfo); break;
            case DamageType.ice: DealIceDamage(damageInfo); break;
            case DamageType.electric: DealElectricDamage(damageInfo); break;
            default: Debug.Log(string.Format("Unrecognized Damage Type: {}",damageInfo.damageType.ToString())); break;
        }

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
            case DamageSource.singleTurret:             ; break;
            case DamageSource.sniperTurret:             ; break;
            case DamageSource.cannonTurret:             ; break;
            case DamageSource.auraTurret:               ; break;
            default: Debug.Log(string.Format("Unrecognized Damage Source: {}", damageInfo.damageSource.ToString())); break;
        }
    }

    protected virtual void DealFireDamage(DamageInfo damageInfo)
    {
        switch (damageInfo.damageSource)
        {
            case DamageSource.singleTurret:; break;
            case DamageSource.sniperTurret:; break;
            case DamageSource.cannonTurret:; break;
            case DamageSource.auraTurret:; break;
            default: Debug.Log(string.Format("Unrecognized Damage Source: {}", damageInfo.damageSource.ToString())); break;
        }
    }

    protected virtual void DealElectricDamage(DamageInfo damageInfo)
    {
        switch (damageInfo.damageSource)
        {
            case DamageSource.singleTurret:; break;
            case DamageSource.sniperTurret:; break;
            case DamageSource.cannonTurret:; break;
            case DamageSource.auraTurret:; break;
            default: Debug.Log(string.Format("Unrecognized Damage Source: {}", damageInfo.damageSource.ToString())); break;
        }
    }

    protected void findNextDestinationPoint()
    {
        // find current and next points from GameManager
        // calculate direction vector between them
        // update movementDirection vector on this 

        Transform currentPoint;

        if (currentPathStage == -1)
        {
            currentPoint = GameManager.Instance.enemySpawnPoint;
            nextPathStage = GameManager.Instance.enemyPathPoints[0];

        }
        else
        {
            currentPoint = nextPathStage;
            if (!isOnLastStage)
            {
                nextPathStage = GameManager.Instance.enemyPathPoints[currentPathStage + 1];

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
}
