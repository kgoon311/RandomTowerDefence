using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObject : BulletBase
{
    [SerializeField] private GameObject boomEffect;
    [SerializeField] private float boomRange;
    protected override void AttackPattern()
    {
        //폭팔 이펙트 소환
        GameObject effect = Instantiate(boomEffect,transform.position,transform.rotation);
        effect.transform.localScale = Vector3.one * boomRange /2;//폭발 범위에 맞게 이펙트 사이즈 변경

        //폭발 범위만큼
        Collider2D[] hitEnemys = Physics2D.OverlapBoxAll(transform.position, Vector2.one * boomRange, 1);
        foreach(Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<EnemyBase>().OnHit(dmg);
        }
    }
}
