using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricBullet : BulletBase
{
    [Header("Stats")]
    [SerializeField] private float electricRange;//전격 공격 범위
    [SerializeField] private int maxAtkCount;//최대 공격 적 카운트
    [SerializeField] private float sternTime;//기절 시간
    private int arrayCount = 0; //배열에 할당된 갯수

    private LineRenderer lineRenderer;//라인랜더러를 복사할 때 사용하는 원본 오브젝트

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }
    protected override void AttackPattern()
    {
        //주변 적 모두 스캔
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(enemyObject.transform.position,
            Vector2.one * electricRange, 1, EnemyLayerMask);

        //가장 가까운 적을 넣는 배열

        Collider2D[] closeEnemy = new Collider2D[maxAtkCount];
        

        //처음 닿은 적에서 부터 가장 가까운 적들 색출
        foreach (Collider2D enemy in hitEnemys)
        {
            //foreach에서 돌고 있는 enemy와 총알에 맞은 hitEnemy와의 거리
            float enemyDis = Vector2.Distance(transform.position, enemy.transform.position);

            //꽉 차지 않았으면 그냥 바로 할당 하고 정렬하기
            if (arrayCount < maxAtkCount)
            {
                closeEnemy[arrayCount] = enemy;
                arrayCount++;
                
                if(arrayCount == maxAtkCount -1)
                    closeEnemy.OrderBy(x => Vector2.Distance(transform.position, x.transform.position));//배열이 꽉 찼을 시 거리가 작은 순으로 정렬
                continue;
            }

            //배열중 가장 먼 적과 총알에 맞은 hitEnemy와의 거리
            float originDis = Vector2.Distance(transform.position, closeEnemy[maxAtkCount - 1].transform.position);
            //마지막 배열보다 가까우면 현재 enemy를 배열에 넣고 정렬
            if (enemyDis < originDis)
            {
                closeEnemy[maxAtkCount - 1] = enemy;

                closeEnemy.OrderBy(x => transform.position - x.transform.position);
            }
        }

        //처음 적 부터 가까운 적들 까지 체인 만들기
        for(int i = 0;i< arrayCount; i++)
        {
            //적 자식에 빈 오브젝트 소환
            GameObject lineObject = new GameObject("Line");
            lineObject.transform.parent = enemyObject.transform;

            LineRenderer line = lineObject.AddComponent<LineRenderer>();
            line.sortingOrder = 1;
            line.materials = lineRenderer.materials;

            //처음 총알 맞은 적과 주변 가까운 적들과 연결
            line.SetPosition(0, enemyObject.transform.position);
            line.SetPosition(1, closeEnemy[i].transform.position);

            EnemyBase enemyBase = closeEnemy[i].GetComponent<EnemyBase>();
            enemyBase.Stern(sternTime);
            enemyBase.OnHit(dmg);

            //모든 체인 삭제
            Destroy(lineObject, 0.5f);
        }
    }
}
