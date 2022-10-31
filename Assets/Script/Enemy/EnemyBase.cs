using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private List<Vector2> movePos;

    [Header("Stats")]
    public List<float> slowDeBuf;
    [SerializeField] float Hp;
    [SerializeField] float speed;
    [SerializeField] private int DropMoney;
    private void Awake()
    {
        movePos = FloorManager.Instance.DiggingFloor;
        StartCoroutine("Move");
    }
    protected virtual void Update()
    {
        if (Hp < 0)
        {
            GameManager.Instance.Monney += DropMoney;
            Destroy(gameObject);
        }
    }
    public void OnHit(float Dmg)
    {
        Hp -= Dmg;
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
                timer -= Time.deltaTime / (speed + slowDeBuf.Sum());
                transform.position = Vector3.Lerp( movePos[i], pos, timer);//타일맵에 맞추기 위해 x,y좌표에 0.5f 더하기
                yield return null;
            }
        }
        yield return null;
    }

   
}
