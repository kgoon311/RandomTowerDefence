using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricBullet : BulletBase
{
    [SerializeField] private LineRenderer lineResnderer;
    [SerializeField] private float electricRange;
    [SerializeField] private int atkCount;
    protected override void Awake()
    {
        base.Awake();
        lineResnderer = GetComponent<LineRenderer>();
    }
    protected override void AttackPattern()
    {
        //�����ȿ� �� ����
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(transform.position, Vector2.one * electricRange, 1, EnemyLayerMask);

        float minDis = electricRange;//���� ����� �Ÿ�
        Collider2D[] closeEnemy = new Collider2D[atkCount];//���� ����� ���� �迭
        int array = 0;
        foreach (Collider2D enemy in hitEnemys)
        {
            if (array < atkCount)
            {
                closeEnemy[array] = enemy;
                array++;

                break;
            }

            //foreach���� ���� �ִ� ������ �Ÿ�
            float enemyDis = Vector2.Distance(transform.position, enemy.transform.position);

            for (int i = 0; i < atkCount; i--)
            {
                //�迭 �ȿ��ִ� ������ �Ÿ�
                float originDis = Vector2.Distance(transform.position, closeEnemy[i].transform.position);
                if (enemyDis < originDis)
                {
                    closeEnemy[i] = enemy;
                    break;
                }
            }
        }
    }
}
