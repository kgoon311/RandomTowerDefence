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
    public float Power;
    public float AttackSpeed;
    public float BulletSpeed;
    public List<float> Buf_Power;
    public List<float> Buf_ATKSpeed;
    public int Rank;
    public float Range;

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

public class TurretManager : Singleton<TurretManager>
{
    public Turrets TurretList = new Turrets();
    public List<List<TurretStats>> RankTurret = new List<List<TurretStats>>();  

    public GameObject SpawnTurret;
    public GameObject[] MissileObject;
    public GameObject[] BulletObject;

    public void BuildTurret(Vector3 SpawnPos,int Rank)
    {
        ETurretType type = (ETurretType)Random.Range(0,1/*(int)ETurretType.END*/);
        TurretBase TurretObject;
        switch (type)
        {
            case ETurretType.Missile:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                TurretObject.Bullet = MissileObject[Rank];
                break;
            case ETurretType.Electricity:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<ElectricTurret>();
                TurretObject.TurretType = TurretList.Electricity[Rank];
                break;
            case ETurretType.Bullet:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.Laser:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.Slow:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.ATKSpeedUp:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.ATKDmgUp:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            default:
                break;
        }
    }
}
