using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int playerHp;
    public int _hp {get{ return playerHp; }  set{ playerHp = value; }}

    private int money;
    public int _money {get { return money; } set { money = value; }}

    public Dictionary<GameObject, EnemyBase> enemyGroup = new Dictionary<GameObject,EnemyBase>();
    public Dictionary<GameObject, TurretBase> turretGroup = new Dictionary<GameObject, TurretBase>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
