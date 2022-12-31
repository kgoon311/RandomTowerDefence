using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObject : BulletBase
{
    [SerializeField] private GameObject boomEffect;
    [SerializeField] private float boomRange;
    public void SettingRange(float range)
    {
        boomRange = range;
    }
    protected override void AttackPattern()
    {
        GameObject effect = Instantiate(boomEffect,transform.position,transform.rotation);  //���� ����Ʈ ��ȯ
        effect.transform.localScale = Vector3.one * boomRange /2;  //���� ������ �°� ����Ʈ ������ ����

        //���� ������ŭ �� ����
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(transform.position, Vector2.one * boomRange, 1, EnemyLayerMask);
        foreach(Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<EnemyBase>().OnHit(dmg);
        }
    }
}
