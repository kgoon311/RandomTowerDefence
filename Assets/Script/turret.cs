using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class turret : MonoBehaviour
{
    public TurretStats TurretType;

    private Vector3 BeforePos;
    private Vector3 MousePos;
    private bool Move;
    private bool MouseOver;
    public GameObject OverTurret;
    private GameObject RangeObject;
    protected virtual void Awake()
    {
        BeforePos = transform.position;
    }
    protected virtual void Start()
    {
        RangeObject = transform.GetChild(0).gameObject;
        RangeObject.transform.localScale = Vector3.one * TurretType.Range;
        RangeObject.SetActive(false);
    }
    protected virtual void Update()
    {
        if (Move)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Floor(MousePos.x) + 0.5f, Mathf.Floor(MousePos.y) + 0.5f, -1);
        }
        if (Input.GetMouseButtonDown(0) && RangeObject.activeSelf == true &&  MouseOver == false)
        {
            RangeObject.SetActive(false);
        }
    }
    private void OnMouseOver()
    {
        MouseOver = true;
        if (Input.GetMouseButtonDown(0))
        {
            Move = true;
            RangeObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (OverTurret == false && Move)
            {
                transform.position = BeforePos;
            }
            else if (Move)
            {
                turret OverTurretScript = OverTurret.GetComponent<turret>();
                if (OverTurretScript.TurretType.Rank == this.TurretType.Rank && OverTurretScript.TurretType.type == this.TurretType.type)
                {
                    RankUp();
                }
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
    private void RankUp()
    {
        TurretManager.instance.AddScript(transform.position, TurretType.Rank + 1);
        Destroy(OverTurret.gameObject);
        Destroy(gameObject.gameObject);
    }
}
public class ATK : turret
{
    private GameObject TargetEnemy;
    protected virtual void SearchEnemy()
    {
    }
    protected void Attack()
    {

    }

}
public class SUP : turret
{

}

