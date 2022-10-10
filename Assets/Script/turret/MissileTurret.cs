using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : ATK
{
    [SerializeField] GameObject MissilePrefab;
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
        GameObject MissileObject = Instantiate(MissilePrefab, transform.position, transform.rotation);

    }
}
