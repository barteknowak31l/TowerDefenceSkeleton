using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] public DamageInfo damageInfo;
    [SerializeField] public float bulletSpeed;

    [Header("Enemy")]
    [SerializeField] protected Transform target;
    [SerializeField] protected LayerMask enemyMask;


    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Rigidbody2D rigid;

    private bool hasEnemyBeenHit = false;

    public virtual void Start()
    {
        Destroy(gameObject,5f);
    }

    protected virtual void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rigid.velocity = direction * bulletSpeed;
    }

    public virtual void FollowTarget(Transform _target)
    {
        target = _target;

    }

    // Damage Info should be set imidiately after Instantiate()
    public void SetDamageInfo(DamageInfo _damageInfo)
    {
        damageInfo = _damageInfo;
    }

    // Implement this to have different outcomes on enemy hit per damage types (i.e ice dmg might apply frost stacks)
    protected abstract void OnEnemyContact(BaseEnemy enemy, DamageInfo damageInfo);

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasEnemyBeenHit)
        {
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                OnEnemyContact(enemy, damageInfo);
                hasEnemyBeenHit = true;

            }

        }
    }
}
