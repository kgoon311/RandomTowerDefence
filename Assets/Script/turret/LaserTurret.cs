using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : ATK
{
    private int layerStack;

    private GameObject chargeParticle;

    private GameObject beforEnemyObject;//같은 적인지 확인할때 사용됩니다
    private EnemyBase enemyScript;

    private LineRenderer lineRenderer;
    private Material m_ChargeMatarial;
    protected override void Start()
    {
        base.Start();
        m_ChargeMatarial = TurretManager.Instance.laserMaterial;
        chargeParticle = TurretManager.Instance.laserParticle;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.material = m_ChargeMatarial;

    }
    protected override void AttackPattern()
    {
        if (TargetEnemy != beforEnemyObject)//적이 달라졌을때 실행
        {
            layerStack = 0;
            beforEnemyObject = TargetEnemy;

            enemyScript = TargetEnemy.GetComponent<EnemyBase>();
        }
        StartCoroutine(LayerAttack());
    }
    private IEnumerator LayerAttack()
    {
        //차지 파티클 오브젝트 소환
        GameObject paricleObject = Instantiate(chargeParticle, transform.position, transform.rotation,transform.parent);

        yield return new WaitForSeconds(1f);

        Destroy(paricleObject);
        if (TargetEnemy == null) yield break;

        lineRenderer.SetPosition(0, TargetEnemy.transform.position);
        enemyScript.OnHit(TurretType.dmg * layerStack++);

        yield return new WaitForSeconds(0.5f);
        lineRenderer.SetPosition(0, transform.position);
    }
}
