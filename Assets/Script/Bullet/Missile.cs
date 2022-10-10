using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float MoveSpeed;
    private float Dmg;
    private Vector3 MoveDis;
    private GameObject EnemyObject;
    void Update()
    {
        if (EnemyObject != null)
        {
            transform.position += MoveDis * MoveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AttackEnemy(GameObject Enemy , float Dmg)
    {
        this.Dmg = Dmg;
        EnemyObject = Enemy;
        MoveDis = (EnemyObject.transform.position - transform.position).normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == EnemyObject)
        {
            Destroy(gameObject);
            EnemyObject.GetComponent<EnemyBase>().OnHit(Dmg);
        }
    }
}
