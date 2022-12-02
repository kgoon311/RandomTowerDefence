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
        movePos = FloorManager.Instance.DiggingFloor;
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
    private IEnumerator HitColor()
    {
        SpriteRenderer color = GetComponent<SpriteRenderer>();
        float timer = 0;
        
        //빨갛게 만들기
        while (timer >= 0)
        {
            timer -= Time.deltaTime * 3;
            color.color = new Color(1, timer, timer);
            yield return null;
        }
        //다시 원래색으로 돌아오게 하기
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
            float timer = 1;
            Vector3 pos = transform.position;
            while (timer > 0)
            {
                timer -= Time.deltaTime / (speed + slowDebuff.Sum());
                transform.position = Vector3.Lerp( movePos[i], pos, timer);//타일맵에 맞추기 위해 x,y좌표에 0.5f 더하기
                yield return null;
            }
        }

        //마지막 지점에 도착했을때
        GameManager.Instance._hp -= 1;
        Destroy(gameObject);

        yield return null;
    }

   
}
