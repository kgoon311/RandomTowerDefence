using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//磐房 辆幅
public enum ETurretType
{
    Missile,
    Electricity,
    Gatling,
    Laser,
    Slow,
    ATKSpeedUp,
    ATKDmgUp,
    END
}

//磐房 胶泡
[System.Serializable]
public class TurretStats
{
    public ETurretType type;
    public Sprite sprite;
    [Header("Stat")]
    public float dmg;
    public float attackSpeed;
    public float bulletSpeed;
    [Header("info")]
    public int rank;
    public float range;
    [Header("buff")]
    public float buf_Power;
    public float buf_ATKSpeed;
}

//葛电 磐房 包府
[System.Serializable]
public class Turrets
{
    public List<TurretStats> Missile;
    public List<TurretStats> Electricity;
    public List<TurretStats> Gatling;
    public List<TurretStats> Laser;
    public List<TurretStats> Slow ;
    public List<TurretStats> ATKSpeedUp;
    public List<TurretStats> ATKDmgUp;
}
[System.Serializable]
public class Bullets
{
    public GameObject[] Missile;
    public GameObject[] Electric;
    public GameObject[] Bullet;

}
public class TurretManager : Singleton<TurretManager>
{
    [Header("Inspector")]
    public Turrets turretList = new Turrets();
    public Bullets bullets;

    [Header ("Laser")]
    public GameObject laserParticle;

    [Header("Object")]
    public GameObject SpawnTurret;

    [SerializeField] ETurretType testTurretidx;

    public void BuildTurret(Vector3 SpawnPos,int Rank)
    {
        ETurretType type = /*(ETurretType)Random.Range(0, (int)ETurretType.END);*/ testTurretidx;
        TurretBase TurretObject = new TurretBase();
        switch (type)
        {
            case ETurretType.Missile:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
                TurretObject.Bullet = bullets.Missile[Rank];
                break;
            case ETurretType.Electricity:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<ElectricTurret>();
                TurretObject.TurretType = turretList.Electricity[Rank];
                TurretObject.Bullet = bullets.Electric[Rank];
                break;
            case ETurretType.Gatling:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
                break;
            case ETurretType.Laser:
                LaserTurret laserTurret = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<LaserTurret>();
                laserTurret.TurretType = turretList.Missile[Rank];
                laserTurret.chargeParticle = laserParticle;
                break;
            case ETurretType.Slow:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
                break;
            case ETurretType.ATKSpeedUp:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
                break;
            case ETurretType.ATKDmgUp:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
                break;
            default:
                break;
        }
    }
}
