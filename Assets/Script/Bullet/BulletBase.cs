using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    private float speed;
    protected float dmg;
    private Vector3 moveDis;
    protected GameObject enemyObject;

    protected LayerMask EnemyLayerMask;
    protected LayerMask TurretLayerMask;
    
    protected virtual void Awake()
    {
        EnemyLayerMask = LayerMask.GetMask("Enemy");
        TurretLayerMask = LayerMask.GetMask("Turret");
    }
    protected virtual void Update()
    {
        Move();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == enemyObject)
        {
            AttackPattern();//총알마다 적에 닿았을때 특수 능력
            Destroy(gameObject);
        }
    }

    //터렛에서 스폰 후 실행하는 셋팅 함수
    public void AttackEnemy(GameObject enemy, float dmg, float speed)
    {
        EnemyLayerMask = LayerMask.GetMask("Enemy");
        TurretLayerMask = LayerMask.GetMask("Turret");

        this.dmg = dmg;
        this.speed = speed;
        enemyObject = enemy;
        moveDis = (enemyObject.transform.position - transform.position).normalized;
    }
    protected virtual void Move()
    {
        if (enemyObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = enemyObject.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position += moveDis * speed * Time.deltaTime;
    }
    protected abstract void AttackPattern();

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
