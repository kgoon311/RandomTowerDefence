using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float MoveSpeed;
    private float Dmg;
    private Vector3 MoveDis;
    private GameObject EnemyObject;
    private void Start()
    {
        Destroy(gameObject, 2);
        Vector2 dir = EnemyObject.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
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
        if(collision.gameObject == EnemyObject)
        {
            Destroy(gameObject);
            EnemyObject.GetComponent<EnemyBase>().OnHit(Dmg);
        }
    }
}
