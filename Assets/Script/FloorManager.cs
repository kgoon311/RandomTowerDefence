using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject FirstDigFloor;
    [SerializeField] private GameObject EndDigFloor;
    [SerializeField] private bool DigEnd;

    private GameObject RecentDigFloor;
    private List<GameObject> DiggingFloor = new List<GameObject>();
    private LayerMask mask;

    void Start()
    {
        mask = LayerMask.GetMask("Floor");
        ResetFloor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && DigEnd == false)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 100000, mask);
            if (hit)
            {
                DigFloor(hit.transform.gameObject);
            }
        }
        if(Input.GetKeyDown(KeyCode.R) && DigEnd == false)
        {
            ResetFloor();
        }
    }
    void ResetFloor()
    {
        RecentDigFloor = FirstDigFloor;
        foreach(GameObject a in DiggingFloor)
        {
            a.GetComponent<SpriteRenderer>().color = Color.white;
            a.GetComponent<turret>().Diging = false;
        }
        DiggingFloor.Clear();
        DiggingFloor.Add(RecentDigFloor);
        RecentDigFloor.GetComponent<SpriteRenderer>().color = Color.clear;
        RecentDigFloor.GetComponent<turret>().Diging = true;
    }
    void DigFloor(GameObject TargetFloor)
    {
        List<GameObject> Fourdirection = new List<GameObject>();// ���ڰ� ������� �޾ƿ���
        bool Dig = false;
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor.transform.position, new Vector2(0.9f, 2.9f), 0, mask))
        {
            Fourdirection.Add(a.transform.gameObject);
        }
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor.transform.position, new Vector2(0.9f, 2.9f), 90, mask))
        {
            Fourdirection.Add(a.transform.gameObject);
        }

        int RecentIdxfloor = Fourdirection.IndexOf(RecentDigFloor);//�ֺ� ���� �ٷ� ���� �� ���� �ִ��� üũ
        if (RecentIdxfloor > -1)//�ִٸ� ����
        {
            Fourdirection.RemoveAt(RecentIdxfloor);//����Ʈ���� �� �� ����
            Fourdirection.RemoveAll(a => a == TargetFloor);//����Ʈ���� �ڽ� �� ����

            foreach (GameObject i in Fourdirection)//�ֺ� ��� ���� �������� ���������� üũ
            {
                if (i.GetComponent<turret>().Diging)
                    Dig = true;
            }
            if (Dig == false)//�������ִٸ� Ŭ���� ���� �ı�
            {
                TargetFloor.GetComponent<turret>().Diging = true;
                TargetFloor.GetComponent<SpriteRenderer>().color = Color.clear;
                DiggingFloor.Add(TargetFloor);
                RecentDigFloor = TargetFloor;
                if (TargetFloor == EndDigFloor)
                    DigEnd = true;
            }
        }
    }

}
