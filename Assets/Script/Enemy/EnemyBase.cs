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

    [SerializeField] private int dropMoney;

    private void Awake()
    {
        GameManager.Instance.enemyGroup.Add(gameObject, this);

        movePos = FloorManager.Instance.DiggingFloor;
        StartCoroutine("Move");
    }
    public void OnHit(float Dmg)
    {
        hp -= Dmg;
        if (hp < 0)
        {
            GameManager.Instance._money += dropMoney;
            Destroy(gameObject);
        }
    }
    private IEnumerator Move()
    {
        int digCount = movePos.Count;

        for (int i = 0; i < digCount; i++)
        {
            float timer = 1;
            Vector3 pos = transform.position;
            while (timer > 0)
            {
                timer -= Time.deltaTime / (speed + slowDebuff.Sum());
                transform.position = Vector3.Lerp( movePos[i], pos, timer);//Ÿ�ϸʿ� ���߱� ���� x,y��ǥ�� 0.5f ���ϱ�
                yield return null;
            }
        }

        //������ ������ ����������
        GameManager.Instance._hp -= 1;
        Destroy(gameObject);

        yield return null;
    }

   
}
