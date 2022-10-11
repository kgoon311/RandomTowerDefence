using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : ATK
{
    public GameObject MissilePrefab;
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
        Missile MissileObject = Instantiate(MissilePrefab, transform.position, transform.rotation).GetComponent<Missile>();
        MissileObject.AttackEnemy(TargetEnemy, TurretType.Power);
    }
}
