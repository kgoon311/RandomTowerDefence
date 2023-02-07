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

    private bool attacking = false;
    protected override void Start()
    {
        base.Start();
        m_ChargeMatarial = TurretManager.Instance.laserMaterial;
        chargeParticle = TurretManager.Instance.laserParticle;

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        lineRenderer.material = m_ChargeMatarial;
    }
    protected override void AttackPattern()
    {
        if (attacking) return;//공격중일 때는 공격x

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
        GameObject paricleObject = Instantiate(chargeParticle, transform.position, transform.rotation,gameObject.transform);

        attacking = true;
        lineRenderer.SetWidth(2, 2);

        yield return new WaitForSeconds(1f);

        Destroy(paricleObject);
        if (TargetEnemy == null)//만약 차징중 적이 사라졌을때 예외처리
        {
            attacking = false;
            yield break;
        }

        float timer = 0;
        Vector2 enemypos = TargetEnemy.gameObject.transform.position;
        while (timer < 1)//적을 향해 레이저를 발사
        {
            timer += Time.deltaTime * 3;
            lineRenderer.SetPosition(0,Vector2.Lerp(transform.position, enemypos, timer));
            yield return null;
        }

        if (enemyScript == null) //만약 레이저 발사중 적이 사라졌을 때 예외처리
        {
            lineRenderer.SetPosition(0, transform.position);    
            attacking = false;
            yield break;
        }
        enemyScript.OnHit(TurretType.dmg * layerStack++);

        timer = 2f;
        while (timer > 0)
        {
            timer -= Time.deltaTime * 4;
            lineRenderer.SetWidth(timer, timer);
            yield return null;
        }

        attacking = false;
        lineRenderer.SetPosition(0, transform.position);
    }
}
