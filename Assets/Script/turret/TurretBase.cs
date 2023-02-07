using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TurretBase : MonoBehaviour
{
    public TurretStats TurretType;

    private bool Move;//�ͷ��� Ŭ���ؼ� �����̴� ���ΰ�
    private Vector3 BeforePos;//���� ��ǥ
    private Vector3 MousePos;//Move�� True�϶� �̵��� ���콺 ��ġ

    private bool MouseOver;//���콺�� �ͷ��� �������� �Ǿ��°�

    private GameObject RangeObject;//���� ������Ʈ
    private GameObject OverTurret;//������ �ͷ� ������Ʈ

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

        //���콺�� �ٸ� ���� Ŭ�������� ����ǥ�� �����
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
                new Vector2(BeforePos.x, BeforePos.y))//������ �� ���� �׳� Ŭ��������
            {
                
                RangeObject.SetActive(true);
            }
            else if (OverTurret == false)//�ٸ� �ͷ��� �� ���� ������
            {
                transform.position = BeforePos;
            }
            else if (OverTurret)//�����̴� ���̰� �ٸ� �ͷ��� ������ ��
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

    //���� ������Ʈ ������ ���� �Լ�
    private void SettingRangeObj()
    {
        RangeObject = transform.GetChild(0).gameObject;//���� ǥ�� ������Ʈ
        RangeObject.transform.localScale = Vector3.one * TurretType.range;//���� ǥ�� ������Ʈ 
        RangeObject.SetActive(false);
    }
    //�巡�׷� ������ �� ���Ǵ� �Լ�
    private void MovePos()
    {
        if (Move)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Floor(MousePos.x) + 0.5f, Mathf.Floor(MousePos.y) + 0.5f, -1);
        }
    }
    //���� ��ް� �������� ���Ǵ� �Լ�
    private void RankUp()
    {
        TurretManager.Instance.BuildTurret(transform.position, TurretType.rank + 1);

        Destroy(OverTurret);
        Destroy(gameObject);
    }

}

public class ATK : TurretBase
{
    [SerializeField] protected GameObject TargetEnemy;//���� Ÿ�� ������Ʈ

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

    //���� �ȿ� �� ã��
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

        float totalATKSpeed = 1f / (TurretType.attackSpeed + TurretType.buf_ATKSpeed); //1�ʵ��� (�⺻ ���� + ����)�� ������

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

