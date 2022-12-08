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
        //범위안에 적 스턴
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(transform.position, Vector2.one * electricRange, 1, EnemyLayerMask);

        float minDis = electricRange;//가장 가까운 거리
        Collider2D[] closeEnemy = new Collider2D[atkCount];//가장 가까운 적들 배열
        int array = 0;
        foreach (Collider2D enemy in hitEnemys)
        {
            if (array < atkCount)
            {
                closeEnemy[array] = enemy;
                array++;

                break;
            }

            //foreach에서 돌고 있는 적과의 거리
            float enemyDis = Vector2.Distance(transform.position, enemy.transform.position);

            for (int i = 0; i < atkCount; i--)
            {
                //배열 안에있는 적과의 거리
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
