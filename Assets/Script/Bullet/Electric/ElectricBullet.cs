using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricBullet : BulletBase
{
    [Header("Stats")]
    [SerializeField] private float electricRange;//���� ���� ����
    [SerializeField] private int maxAtkCount;//�ִ� ���� �� ī��Ʈ
    [SerializeField] private float sternTime;//���� �ð�
    private int arrayCount = 0; //�迭�� �Ҵ�� ����

    private LineRenderer lineRenderer;//���η������� ������ �� ����ϴ� ���� ������Ʈ

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }
    protected override void AttackPattern()
    {
        //�ֺ� �� ��� ��ĵ
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(enemyObject.transform.position,
            Vector2.one * electricRange, 1, EnemyLayerMask);

        //���� ����� ���� �ִ� �迭

        Collider2D[] closeEnemy = new Collider2D[maxAtkCount];
        

        //ó�� ���� ������ ���� ���� ����� ���� ����
        foreach (Collider2D enemy in hitEnemys)
        {
            //foreach���� ���� �ִ� enemy�� �Ѿ˿� ���� hitEnemy���� �Ÿ�
            float enemyDis = Vector2.Distance(transform.position, enemy.transform.position);

            //�� ���� �ʾ����� �׳� �ٷ� �Ҵ� �ϰ� �����ϱ�
            if (arrayCount < maxAtkCount)
            {
                closeEnemy[arrayCount] = enemy;
                arrayCount++;
                
                if(arrayCount == maxAtkCount -1)
                    closeEnemy.OrderBy(x => Vector2.Distance(transform.position, x.transform.position));//�迭�� �� á�� �� �Ÿ��� ���� ������ ����
                continue;
            }

            //�迭�� ���� �� ���� �Ѿ˿� ���� hitEnemy���� �Ÿ�
            float originDis = Vector2.Distance(transform.position, closeEnemy[maxAtkCount - 1].transform.position);
            //������ �迭���� ������ ���� enemy�� �迭�� �ְ� ����
            if (enemyDis < originDis)
            {
                closeEnemy[maxAtkCount - 1] = enemy;

                closeEnemy.OrderBy(x => transform.position - x.transform.position);
            }
        }

        //ó�� �� ���� ����� ���� ���� ü�� �����
        for(int i = 0;i< arrayCount; i++)
        {
            //�� �ڽĿ� �� ������Ʈ ��ȯ
            GameObject lineObject = new GameObject("Line");
            lineObject.transform.parent = enemyObject.transform;

            LineRenderer line = lineObject.AddComponent<LineRenderer>();
            line.sortingOrder = 1;
            line.materials = lineRenderer.materials;

            //ó�� �Ѿ� ���� ���� �ֺ� ����� ����� ����
            line.SetPosition(0, enemyObject.transform.position);
            line.SetPosition(1, closeEnemy[i].transform.position);

            EnemyBase enemyBase = closeEnemy[i].GetComponent<EnemyBase>();
            enemyBase.Stern(sternTime);
            enemyBase.OnHit(dmg);

            //��� ü�� ����
            Destroy(lineObject, 0.5f);
        }
    }
}
