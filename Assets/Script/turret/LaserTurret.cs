using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : ATK
{
    private int layerCount;

    public ParticleSystem chargeParticle;

    private GameObject beforEnemyObject;//같은 적인지 확인할때 사용됩니다
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
        if (TargetEnemy != beforEnemyObject)//적이 달라졌을때 실행
        {
            layerCount = 0;
            beforEnemyObject = TargetEnemy;

            enemyScript = TargetEnemy.GetComponent<EnemyBase>();
        }
        StartCoroutine(LayerAttack());
    }
    private IEnumerator LayerAttack()
    {
        chargeParticle.Play();
        yield return new WaitForSeconds(0.5f);
        chargeParticle.Stop();

        lineRenderer.SetPosition(1, TargetEnemy.transform.position);
        TargetEnemy.GetComponent<EnemyBase>();

        enemyScript.OnHit(TurretType.dmg + 5 * layerCount);

        layerCount++;
        yield return new WaitForSeconds(0.5f);
        lineRenderer.SetPosition(1, transform.position);
    }
}
