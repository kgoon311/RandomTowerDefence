using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
[System.Serializable]
public class TurretStats
{
    public ETurretType type;
    public float Power;
    public float AttackSpeed;
    public List<float> Buf_Power;
    public List<float> Buf_ATKSpeed;
    public int Rank;
    public float Range;

}
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
    ETurretType a;
    public Turrets TurretList = new Turrets();
/*    public List<List<TurretStats>> RankTurret = new List<List<TurretStats>>();*/
    //public Dictionary<int, List<TurretStats>> RankTurret = new Dictionary<int, List<TurretStats>>();
    public GameObject SpawnTurret;
    public GameObject MissileObject;
    public GameObject BulletObject;
    public void AddScript(Vector3 SpawnPos,int Rank)
    {
        ETurretType type = (ETurretType)Random.Range(0, 1/*(int)ETurretType.END*/);
        TurretBase TurretObject;
        switch (type)
        {
            case ETurretType.Missile:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                TurretObject.Bullet = MissileObject;
                break;
            case ETurretType.Electricity:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<MissileTurret>();
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
