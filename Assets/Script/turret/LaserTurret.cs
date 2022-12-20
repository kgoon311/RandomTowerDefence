using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : ATK
{
    private int LayerCount;

    private GameObject BeforEnemyObject;//같은 적인지 확인할때 사용됩니다
    private LineRenderer lineRenderer;
    private EnemyBase enemyScript;
    protected override void Start()
    {
        base.Start();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
    }
    protected override void AttackPattern()
    {
        if (TargetEnemy != BeforEnemyObject)
        {
            LayerCount = 0;
            BeforEnemyObject = TargetEnemy;

            enemyScript = TargetEnemy.GetComponent<EnemyBase>();
            lineRenderer.SetPosition(1, transform.position);

        }
        else
        {
            lineRenderer.SetPosition(1, TargetEnemy.transform.position);
            TargetEnemy.GetComponent<EnemyBase>();

            enemyScript.OnHit(TurretType.dmg + 5 * LayerCount);

            LayerCount++;
        }
    }
}
