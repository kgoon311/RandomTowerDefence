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
        List<GameObject> Fourdirection = new List<GameObject>();// 십자가 모양으로 받아오기
        bool Dig = false;
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor.transform.position, new Vector2(0.9f, 2.9f), 0, mask))
        {
            Fourdirection.Add(a.transform.gameObject);
        }
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor.transform.position, new Vector2(0.9f, 2.9f), 90, mask))
        {
            Fourdirection.Add(a.transform.gameObject);
        }

        int RecentIdxfloor = Fourdirection.IndexOf(RecentDigFloor);//주변 땅중 바로 전에 판 땅이 있는지 체크
        if (RecentIdxfloor > -1)//있다면 실행
        {
            Fourdirection.RemoveAt(RecentIdxfloor);//리스트에서 전 땅 삭제
            Fourdirection.RemoveAll(a => a == TargetFloor);//리스트에서 자신 땅 삭제

            foreach (GameObject i in Fourdirection)//주변 모든 땅이 파졌는지 안파졌는지 체크
            {
                if (i.GetComponent<turret>().Diging)
                    Dig = true;
            }
            if (Dig == false)//안파져있다면 클릭한 땅을 파기
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
