using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    private float speed;
    private float dmg;
    private Vector3 moveDis;
    private GameObject enemyObject;

    //�ͷ����� ���� �� �����ϴ� ���� �Լ�
    public void AttackEnemy(GameObject enemy, float dmg , float speed)
    {
        this.dmg = dmg;
        this.speed = speed;
        enemyObject = enemy;
        moveDis = (enemyObject.transform.position - transform.position).normalized;
    }

    protected virtual void Update()
    {
        if (enemyObject == null)
            Destroy(gameObject);

        Vector2 dir = enemyObject.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position += moveDis * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == enemyObject)
        {
            AttackPattern();
            Destroy(gameObject);
        }
    }
    protected abstract void AttackPattern();

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}