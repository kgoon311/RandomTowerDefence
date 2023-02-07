using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : ATK
{
    private int layerStack;

    private GameObject chargeParticle;

    private GameObject beforEnemyObject;//���� ������ Ȯ���Ҷ� ���˴ϴ�
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
        if (attacking) return;//�������� ���� ����x

        if (TargetEnemy != beforEnemyObject)//���� �޶������� ����
        {
            layerStack = 0;
            beforEnemyObject = TargetEnemy;

            enemyScript = TargetEnemy.GetComponent<EnemyBase>();
        }
        StartCoroutine(LayerAttack());
    }
    private IEnumerator LayerAttack()
    {
        //���� ��ƼŬ ������Ʈ ��ȯ
        GameObject paricleObject = Instantiate(chargeParticle, transform.position, transform.rotation,gameObject.transform);

        attacking = true;
        lineRenderer.SetWidth(2, 2);

        yield return new WaitForSeconds(1f);

        Destroy(paricleObject);
        if (TargetEnemy == null)//���� ��¡�� ���� ��������� ����ó��
        {
            attacking = false;
            yield break;
        }

        float timer = 0;
        Vector2 enemypos = TargetEnemy.gameObject.transform.position;
        while (timer < 1)//���� ���� �������� �߻�
        {
            timer += Time.deltaTime * 3;
            lineRenderer.SetPosition(0,Vector2.Lerp(transform.position, enemypos, timer));
            yield return null;
        }

        if (enemyScript == null) //���� ������ �߻��� ���� ������� �� ����ó��
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
