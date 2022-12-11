using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTurret : ATK
{
    public ParticleSystem particle;
    protected override void AttackPattern()
    {
        BulletBase MissileObject = Instantiate(Bullet, transform.position, transform.rotation).GetComponent<BulletBase>();
        MissileObject.AttackEnemy(TargetEnemy, TurretType.dmg, TurretType.bulletSpeed);
    }
}
