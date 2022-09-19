using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Missile,
    Electricity,
    bullet,
    Laser,
    Slow,
    ATKSpeedUp,
    ATKDmgUp,
}
public class turret : MonoBehaviour
{
    public Type TurretType;
    public int Dmg;
    public int ATKSpeed;
    public int Buf_ATKSpeed;
    public int Rank;
    private Vector3 BeforePos;
    private Vector3 MousePos;
    private bool Move;
    private void Update()
    {
        if(Move)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Floor(MousePos.x) + 0.5f, Mathf.Floor(MousePos.y) + 0.5f, -1);
        }
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Move = true;
            BeforePos = transform.position;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Move = false;
            transform.position = BeforePos;
        }
    }
}
abstract class ATK : turret
{
    protected void Start()
    {
        StartCoroutine("Attack");
    }
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(ATKSpeed - Buf_ATKSpeed);
        AttackPattern();
        yield return StartCoroutine("Attack");
    }
    abstract protected void AttackPattern();
}
abstract class Buffer : turret
{

}

