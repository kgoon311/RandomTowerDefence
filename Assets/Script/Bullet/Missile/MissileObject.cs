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
        GameObject effect = Instantiate(boomEffect,transform.position,transform.rotation);  //폭팔 이펙트 소환
        effect.transform.localScale = Vector3.one * boomRange /2;  //폭발 범위에 맞게 이펙트 사이즈 변경

        //폭발 범위만큼 적 공격
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(transform.position, Vector2.one * boomRange, 1, EnemyLayerMask);
        foreach(Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<EnemyBase>().OnHit(dmg);
        }
    }
}
