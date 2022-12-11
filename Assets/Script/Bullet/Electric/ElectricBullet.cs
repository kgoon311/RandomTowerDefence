using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricBullet : BulletBase
{
    private LineRenderer lineResnderer;
    [SerializeField] private float electricRange;
    [SerializeField] private int atkCount;
    protected override void Awake()
    {
        base.Awake();
        lineResnderer = GetComponent<LineRenderer>();
    }
    protected override void AttackPattern()
    {
        //�ֺ� �� ��� ��ĵ
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(enemyObject.transform.position,
            Vector2.one * electricRange, 1, EnemyLayerMask);

        float minDis = electricRange;//���� ����� �Ÿ�
        Collider2D[] closeEnemy = new Collider2D[atkCount];//���� ����� ���� �迭
        int array = 0;//�迭�� �Ҵ�� ����
        foreach (Collider2D enemy in hitEnemys)
        {
            //foreach���� ���� �ִ� ������ �Ÿ�
            float enemyDis = Vector2.Distance(transform.position, enemy.transform.position);
            if (array < atkCount)
            {
                closeEnemy[array] = enemy;
                array++;

                closeEnemy.OrderBy(x => transform.position - x.transform.position);
                break;
            }
            for (int i = atkCount - 1; i >= 0; i--)
            {
                //�迭 �ȿ��ִ� ������ �Ÿ�
                float originDis = Vector2.Distance(transform.position, closeEnemy[i].transform.position);
                if (enemyDis < originDis)
                {
                    closeEnemy[i] = enemy;

                    closeEnemy.OrderBy(x => transform.position - x.transform.position);
                    break;
                }
            }
        }
    }
}
