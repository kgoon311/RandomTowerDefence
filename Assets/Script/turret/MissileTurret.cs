using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : ATK
{
    protected override void AttackPattern()
    {
        BulletBase MissileObject = Instantiate(Bullet, transform.position, transform.rotation).GetComponent<BulletBase>();
        MissileObject.AttackEnemy(TargetEnemy, TurretType.dmg , TurretType.bulletSpeed);
    }
}
