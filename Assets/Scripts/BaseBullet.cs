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

    [Header("Colors")]
    [SerializeField] protected Color fireColor;
    [SerializeField] protected Color iceColor;
    [SerializeField] protected Color normalColor;


	public BaseEnemy enemyToIgnore;

    public virtual void Start()
    {
        Destroy(gameObject,5f);

        spriteRenderer = GetComponent<SpriteRenderer>();

        setColor();

    }

    private void setColor()
    {
        switch (damageInfo.damageType)
        {
            case DamageType.fire: spriteRenderer.color = fireColor; break;
            case DamageType.ice: spriteRenderer.color = iceColor; break;
            case DamageType.normal: spriteRenderer.color = normalColor; break;
        }
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
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy == null) return;
            OnEnemyContact(enemy, damageInfo);

        }

}
