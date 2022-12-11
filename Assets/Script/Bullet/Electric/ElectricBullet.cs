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
        //주변 적 모두 스캔
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(enemyObject.transform.position,
            Vector2.one * electricRange, 1, EnemyLayerMask);

        float minDis = electricRange;//가장 가까운 거리
        Collider2D[] closeEnemy = new Collider2D[atkCount];//가장 가까운 적들 배열
        int array = 0;//배열에 할당된 갯수
        foreach (Collider2D enemy in hitEnemys)
        {
            //foreach에서 돌고 있는 적과의 거리
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
                //배열 안에있는 적과의 거리
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
