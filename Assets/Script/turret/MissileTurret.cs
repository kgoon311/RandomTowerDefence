using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : ATK
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void AttackPattern()
    {
        Missile MissileObject = Instantiate(Bullet, transform.position, transform.rotation).GetComponent<Missile>();
        MissileObject.AttackEnemy(TargetEnemy, TurretType.Power);
    }
}
