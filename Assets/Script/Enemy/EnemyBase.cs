using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private List<Vector2> movePos;

    [Header("Stats")]
    public List<float> slowDebuff;

    [SerializeField] private float hp;
    [SerializeField] private float speed;
    private float orignSpeed;

    [SerializeField] private int dropMoney;

    Coroutine strun;

    private void Awake()
    {
        movePos = FloorManager.Instance.DiggingFloor;
        orignSpeed = speed;
        StartCoroutine("Move");
    }

    public void OnHit(float Dmg)
    {
        hp -= Dmg;
        StartCoroutine(HitColor());
        if (hp < 0)
        {
            GameManager.Instance._money += dropMoney;
            Destroy(gameObject);
        }
    }

    public void Stern(float sternTime)
    {
        if(strun != null)
            StopCoroutine(strun);
        strun = StartCoroutine(C_Stern(sternTime));
    }
    private IEnumerator C_Stern(float sternTime)
    {
        speed = 0;
        yield return new WaitForSeconds(sternTime);
        speed = orignSpeed;
    }
    private IEnumerator HitColor()
    {
        SpriteRenderer color = GetComponent<SpriteRenderer>();
        float timer = 0;
        
        //������ �����
        while (timer >= 0)
        {
            timer -= Time.deltaTime * 3;
            color.color = new Color(1, timer, timer);
            yield return null;
        }
        //�ٽ� ���������� ���ƿ��� �ϱ�
        while(timer <= 1)
        {
            timer += Time.deltaTime*3;
            color.color = new Color(1, timer, timer);
            yield return null;
        }

        yield return null;
    }
    private IEnumerator Move()
    {
        int digCount = movePos.Count;
        for (int i = 0; i < digCount; i++)
        {
            float timer = 0;
            Vector2 pos = transform.position;

            while (timer < 1)
            {
                timer += Time.deltaTime * speed / (slowDebuff.Sum() + 1);
                transform.position = Vector3.Lerp( pos, movePos[i], timer);//Ÿ�ϸʿ� ���߱� ���� x,y��ǥ�� 0.5f ���ϱ�

                Vector2 dir = movePos[i] - pos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                yield return null;
            }
        }

        //������ ������ ����������
        GameManager.Instance._hp -= 1;
        Destroy(gameObject);

        yield return null;
    }

   
}
