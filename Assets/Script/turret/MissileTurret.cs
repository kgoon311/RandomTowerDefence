using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : ATK
{
    protected override void AttackPattern()
    {
        MissileObject MissileObject = Instantiate(Bullet, transform.position, transform.rotation).GetComponent<MissileObject>();
        MissileObject.AttackEnemy(TargetEnemy, TurretType.Power);
    }
}
