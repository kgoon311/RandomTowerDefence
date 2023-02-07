using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TurretBase : MonoBehaviour
{
    public TurretStats TurretType;

    private bool Move;//터렛을 클릭해서 움직이는 중인가
    private Vector3 BeforePos;//원래 좌표
    private Vector3 MousePos;//Move가 True일때 이동할 마우스 위치

    private bool MouseOver;//마우스가 터렛과 오버레이 되었는가

    private GameObject RangeObject;//범위 오브젝트
    private GameObject OverTurret;//겹쳐진 터렛 오브젝트

    protected LayerMask EnemyLayerMask;
    protected LayerMask TurretLayerMask;

    protected List<Collider2D> searchObjects = new List<Collider2D>();   
    
    public GameObject Bullet;

    protected virtual void Awake()
    {
        BeforePos = transform.position;


        EnemyLayerMask = LayerMask.GetMask("Enemy");
        TurretLayerMask = LayerMask.GetMask("Turret");
    }
    protected virtual void Start()
    {
        SettingRangeObj();
        GetComponent<SpriteRenderer>().sprite = TurretType.sprite;

    }
    protected virtual void Update()
    {
        MovePos();

        //마우스가 다른 곳을 클릭했을때 범위표시 지우기
        if (MouseOver == false && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            RangeObject.SetActive(false);
    }
   
    private void OnMouseOver()
    {
        MouseOver = true;
        if (Input.GetMouseButtonDown(0))
        {
            Move = true;
        }
        else if (Input.GetMouseButtonUp(0) && Move)
        {
            if (new Vector2(transform.position.x,transform.position.y) == 
                new Vector2(BeforePos.x, BeforePos.y))//움직인 것 없이 그냥 클릭했을때
            {
                
                RangeObject.SetActive(true);
            }
            else if (OverTurret == false)//다른 터렛에 안 겹쳐 있을때
            {
                transform.position = BeforePos;
            }
            else if (OverTurret)//움직이는 중이고 다른 터렛에 겹쳤을 때
            {
                TurretBase OverTurretScript = OverTurret.GetComponent<TurretBase>();
                if (OverTurretScript.TurretType.rank == this.TurretType.rank && OverTurretScript.TurretType.type == this.TurretType.type)
                {
                    RankUp();
                }
                else
                    transform.position = BeforePos;
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

    //범위 오브젝트 사이즈 셋팅 함수
    private void SettingRangeObj()
    {
        RangeObject = transform.GetChild(0).gameObject;//범위 표시 오브젝트
        RangeObject.transform.localScale = Vector3.one * TurretType.range;//범위 표시 오브젝트 
        RangeObject.SetActive(false);
    }
    //드래그로 움직일 때 사용되는 함수
    private void MovePos()
    {
        if (Move)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Floor(MousePos.x) + 0.5f, Mathf.Floor(MousePos.y) + 0.5f, -1);
        }
    }
    //같은 등급과 합쳐질때 사용되는 함수
    private void RankUp()
    {
        TurretManager.Instance.BuildTurret(transform.position, TurretType.rank + 1);

        Destroy(OverTurret);
        Destroy(gameObject);
    }

}

public class ATK : TurretBase
{
    [SerializeField] protected GameObject TargetEnemy;//공격 타겟 오브젝트

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Attack());
    }

    protected override void Update()
    {
        base.Update();
        SearchEnemy();
    }

    //범위 안에 적 찾기
    protected virtual void SearchEnemy()
    {
        searchObjects = new List<Collider2D>(Physics2D.OverlapBoxAll(transform.position, 
            new Vector2(TurretType.range, TurretType.range), 0, EnemyLayerMask));

        if (TargetEnemy == null && searchObjects != null)
        {
            float EnemyPos = Mathf.Infinity;

            foreach (Collider2D Enemy in searchObjects)
            {
                float Distance = Vector2.Distance(transform.position, Enemy.transform.position);
                if (Distance < EnemyPos)
                {
                    EnemyPos = Distance;
                    TargetEnemy = Enemy.gameObject;
                }
            }
        }
        else if (TargetEnemy != null)
        {
            Vector2 dir = TargetEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + -90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (!searchObjects.Find((x) => x.gameObject == TargetEnemy))
        {
            TargetEnemy = null;
        }
    }

    private IEnumerator Attack()
    {
        if (TargetEnemy != null)
        {
            AttackPattern();
        }

        float totalATKSpeed = 1f / (TurretType.attackSpeed + TurretType.buf_ATKSpeed); //1초동안 (기본 공속 + 버프)번 때리기

        yield return new WaitForSeconds(totalATKSpeed);
        StartCoroutine(Attack());
    }

    protected virtual void AttackPattern() {
        BulletBase BulletObject = Instantiate(Bullet, transform.position, transform.rotation).GetComponent<BulletBase>();
        BulletObject.AttackEnemy(TargetEnemy, TurretType.dmg, TurretType.bulletSpeed);
        BulletObject.transform.parent = gameObject.transform;
    }
}
public class SUP : TurretBase
{

}

