using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TurretBase : MonoBehaviour
{
    public TurretStats TurretType;

    private bool Move;//움직이는지
    private Vector3 BeforePos;//움직일때 원래 자리
    private Vector3 MousePos;//Move가 True일때 이동할 마우스 위치


    private bool MouseOver;

    private GameObject OverTurret;
    private GameObject RangeObject;

    protected LayerMask EnemyLayer;
    protected LayerMask TurretLayer;

    public GameObject Bullet;

    protected virtual void Awake()
    {
        GameManager.Instance.turretGroup.Add(gameObject, this);//
        BeforePos = transform.position;

        EnemyLayer = LayerMask.GetMask("Enemy");
        TurretLayer = LayerMask.GetMask("Turret");

        SettingRangeObj();
    }

    protected virtual void Update()
    {
        MovePos();

        if (MouseOver == false && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))//마우스가 다른 곳을 클릭했을때 범위표시 지우기
            RangeObject.SetActive(false);
    }
   
    private void OnMouseOver()
    {
        MouseOver = true;
        if (Input.GetMouseButtonDown(0))
        {
            Move = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (OverTurret == false && Move)
            {
                transform.position = BeforePos;
            }
            else if (Move)
            {
                TurretBase OverTurretScript = OverTurret.GetComponent<TurretBase>();
                if (OverTurretScript.TurretType.Rank == this.TurretType.Rank && OverTurretScript.TurretType.type == this.TurretType.type)
                {
                    RankUp();
                }
            }
            else
            {
                RangeObject.SetActive(true);
            }
            Move = false;
        }
    }
    private void OnMouseExit()
    {
        MouseOver = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Turret"))
        {
            OverTurret = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Turret"))
        {
            OverTurret = null;
        }
    }
    private void SettingRangeObj()
    {
        RangeObject = transform.GetChild(0).gameObject;//범위 표시 오브젝트
        RangeObject.transform.localScale = Vector3.one * TurretType.Range;//범위 표시 오브젝트 
        RangeObject.SetActive(false);
    }
    private void MovePos()
    {
        if (Move)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Floor(MousePos.x) + 0.5f, Mathf.Floor(MousePos.y) + 0.5f, -1);
        }
    }
    private void RankUp()
    {
        TurretManager.Instance.AddScript(transform.position, TurretType.Rank + 1);
        Destroy(OverTurret.gameObject);
        Destroy(gameObject.gameObject);
    }
}
public class ATK : TurretBase
{
    protected GameObject TargetEnemy;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Attack());
    }
    protected override void Update()
    {
        base.Update();
        SearchEnemy();
    }
    protected virtual void SearchEnemy()
    {
        List<Collider2D> HitEnemys = new List<Collider2D>(Physics2D.OverlapBoxAll
           (transform.position, new Vector2(TurretType.Range, TurretType.Range), 0, EnemyLayer));
        if (TargetEnemy == null && HitEnemys != null)
        {
            float EnemyPos = Mathf.Infinity;
            foreach (Collider2D Enemy in HitEnemys)
            {
                float Distance = Vector2.Distance(transform.position, Enemy.transform.position);
                if (Distance < EnemyPos)
                {
                    EnemyPos = Distance;
                    TargetEnemy = Enemy.gameObject;
                }
            }
        }
        else if (!HitEnemys.Find((x) => x.gameObject == TargetEnemy))
        {
            TargetEnemy = null;
        }
    }
    private IEnumerator Attack()
    {
        if (TargetEnemy != null)
        {
            AttackPattern();
            Debug.Log("test");
        }
        float SumBufSpeed = TurretType.Buf_ATKSpeed.Sum();
        float ATKSpeed = 1 / TurretType.AttackSpeed;
        yield return new WaitForSeconds(ATKSpeed);
        yield return StartCoroutine(Attack());
    }
    protected virtual void AttackPattern() { }

}
public class SUP : TurretBase
{

}

