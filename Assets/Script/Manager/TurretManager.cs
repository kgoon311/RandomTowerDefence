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
    public int Rank;
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
public class TurretManager : MonoBehaviour
{
    ETurretType a;
    public Turrets TurretList = new Turrets();
/*    public List<List<TurretStats>> RankTurret = new List<List<TurretStats>>();*/
    //public Dictionary<int, List<TurretStats>> RankTurret = new Dictionary<int, List<TurretStats>>();
    public GameObject SpawnTurret;
    public static TurretManager instance { get; set; }
    private void Awake()
    {
        instance = this;
    }
    public void AddScript(Vector3 SpawnPos,int Rank)
    {
        ETurretType type = (ETurretType)Random.Range(0, (int)ETurretType.END);
        Missile TurretObject;
        switch (type)
        {
            case ETurretType.Missile:
                TurretObject = Instantiate(SpawnTurret,SpawnPos,transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.Electricity:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.Bullet:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.Laser:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.Slow:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.ATKSpeedUp:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            case ETurretType.ATKDmgUp:
                TurretObject = Instantiate(SpawnTurret, SpawnPos, transform.rotation).AddComponent<Missile>();
                TurretObject.TurretType = TurretList.Missile[Rank];
                break;
            default:
                break;
        }
    }
}
