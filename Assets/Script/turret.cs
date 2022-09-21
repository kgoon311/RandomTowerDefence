using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class turret : MonoBehaviour
{
    public TurretStats TurretType;
    public int Power;
    public List<int> Buf_Power;


    private Vector3 BeforePos;
    private Vector3 MousePos;
    private bool Move;
    public GameObject OverTurret;
    protected virtual void Awake()
    {
        BeforePos = transform.position;
    }
    protected virtual void Update()
    {
        if (Move)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Floor(MousePos.x) + 0.5f, Mathf.Floor(MousePos.y) + 0.5f, -1);
        }

    }
    private void OnMouseOver()
    {
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
                turret OverTurretScript = OverTurret.GetComponent<turret>();
                if(OverTurretScript.TurretType.Rank == this.TurretType.Rank && OverTurretScript.TurretType.type == this.TurretType.type)
                {
                    RankUp();
                }
            }
            Move = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turret")
        {
            OverTurret = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Turret")
        {
            OverTurret = null;
        }
    }
    private void RankUp()
    {
        TurretManager.instance.AddScript(transform.position, TurretType.Rank+1);
        Destroy(OverTurret.gameObject);
        Destroy(gameObject.gameObject);
    }
}
public class ATK : turret
{
    public int ATKSpeed;
    public List<int> Buf_ATKSpeed;
}
public class SUP : turret
{

}

