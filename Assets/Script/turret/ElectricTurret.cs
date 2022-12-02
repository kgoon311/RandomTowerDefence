using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTurret : ATK
{
    protected override void AttackPattern()
    {
        StartCoroutine(ChageBullet());
        /*Collider2D[] aroundEnemys = Physics2D.OverlapBoxAll(TargetEnemy.transform.position, Vector2.one * 3 , 0,EnemyLayerMask);
        foreach(Collider2D enemy in aroundEnemys)
        {
           TargetEnemy.GetComponent<EnemyBase>().OnHit(TurretType.Power);
        }*/
    }
    private IEnumerator ChageBullet()
    {
        yield return new WaitForSeconds(1f);
        BulletBase MissileObject = Instantiate(Bullet, transform.position, transform.rotation).GetComponent<BulletBase>();
        MissileObject.AttackEnemy(TargetEnemy, TurretType.Power, TurretType.BulletSpeed);
    }
}
