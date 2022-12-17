using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ͷ� ����
public enum ETurretType
{
    Missile,
    Electricity,
    Bullet,
    Laser,
    Slow,
    ATKSpeedUp,
    ATKDmgUp,
    END
}

//�ͷ� ����
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

//��� �ͷ� ����
[System.Serializable]
public class Turrets
{
    public List<TurretStats> Missile;
    public List<TurretStats> Electricity;
    public List<TurretStats> Bullet;
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
    public Turrets turretList = new Turrets();
    public Bullets bullets;

    public GameObject SpawnTurret;

    [SerializeField] int testTurretidx;

    public void BuildTurret(Vector3 SpawnPos,int Rank)
    {
        ETurretType type = (ETurretType)Random.Range(0, (int)ETurretType.END); /*(ETurretType)testTurretidx;*/
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
            case ETurretType.Bullet:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
                break;
            case ETurretType.Laser:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = turretList.Missile[Rank];
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
