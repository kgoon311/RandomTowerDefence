using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorManager : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] Tilemap GrassTileMap;
    [SerializeField] Tilemap DigTileMap;
    [SerializeField] Tilemap FakeDirt;//화면 상으로만 바로 전 블록 파져있게
    [SerializeField] Tile Grass;
    [SerializeField] Tile Dirt;
    [Header("Floor")]
    [SerializeField] private Vector2 FirstDigFloorPos;
    [SerializeField] private Vector2 EndDigFloorPos;
    [SerializeField] private bool DigEnd;

    private Vector2 RecentDigFloor;
    private List<Vector3Int> DiggingFloor = new List<Vector3Int>();
    private LayerMask Grassmask;
    private LayerMask Digmask;

    [Header("MousePointer")]
    [SerializeField] GameObject M_Object;
    [SerializeField] Sprite[] M_Images = new Sprite[2];
    private SpriteRenderer M_Sprite;
    private Vector3 mousePosition;

    void Start()
    {
        Grassmask = LayerMask.GetMask("Floor") | LayerMask.GetMask("Turret");
        Digmask = LayerMask.GetMask("DigFloor");
        M_Sprite = M_Object.GetComponent<SpriteRenderer>();
        ResetFloor();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(Mathf.Floor(mousePosition.x) + 0.5f, Mathf.Floor(mousePosition.y) + 0.5f);
        if (Input.GetMouseButtonDown(0) && DigEnd == false)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 100000f, Grassmask);
            if (hit)
            {
                DigFloor(mousePosition);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && DigEnd == false)
        {
            ResetFloor();
        }
        MousePointerSpawn();
    }
    void MousePointerSpawn()
    {
        M_Object.transform.position = mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 10000f, Grassmask);
        Collider2D hit1 = Physics2D.OverlapBox(mousePosition, new Vector2(0.9f, 2.9f), 0, Digmask);//세로 오버랩박스
        Collider2D hit2 = Physics2D.OverlapBox(mousePosition, new Vector2(2.9f, 0.9f), 0, Digmask);//가로 오버렙박스
        if (hit&&hit1 ==null&&hit2==null && Mathf.Sqrt(Mathf.Pow(RecentDigFloor.x - mousePosition.x, 2) + Mathf.Pow(RecentDigFloor.y - mousePosition.y, 2)) == 1)
        {
            M_Sprite.sprite = M_Images[0];
        }
        else
        {
            M_Sprite.sprite = M_Images[1];
        }
    }
    void ResetFloor()
    {
        RecentDigFloor = FirstDigFloorPos;
        Vector3Int TilePos;
        foreach (Vector3Int a in DiggingFloor)
        {
            TilePos = GrassTileMap.WorldToCell(a);
            GrassTileMap.SetTile(TilePos, Grass);
            DigTileMap.SetTile(TilePos, null);
            FakeDirt.SetTile(TilePos, Dirt);
        }
        TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
        DigTileMap.SetTile(TilePos, null);
        DiggingFloor.Clear();
    }
    void DigFloor(Vector2 TargetFloor)
    {
        /* List<GameObject> Fourdirection = new List<GameObject>();// 십자가 모양으로 받아오기
          bool Dig = false;*/
        Collider2D hit = Physics2D.OverlapBox(TargetFloor, new Vector2(0.9f, 2.9f), 0, Digmask);//세로 오버랩박스
        Collider2D hit2 = Physics2D.OverlapBox(TargetFloor, new Vector2(2.9f, 0.9f), 0, Digmask);//가로 오버렙박스
        if (hit == null && hit2 == null && Mathf.Sqrt(Mathf.Pow(RecentDigFloor.x - TargetFloor.x, 2) + Mathf.Pow(RecentDigFloor.y - TargetFloor.y, 2)) == 1)//바로 전에 자신의 블록이 있는지 체크
        {
            Vector3Int TilePos = GrassTileMap.WorldToCell(TargetFloor);
            DiggingFloor.Add(TilePos);
            GrassTileMap.SetTile(TilePos, null);
            if (EndDigFloorPos != TargetFloor)//마지막 블록이 아니라면 페이크 흙 설치
            {
                FakeDirt.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                RecentDigFloor = TargetFloor;
            }
            else
            {
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(TargetFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                DigEnd = true;
            }

        }
        /*foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor, new Vector2(0.9f, 2.9f), 0, Digmask))//세로 오버랩박스
        {
            Fourdirection.Add(a.transform.gameObject);
        }
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor, new Vector2(0.9f, 2.9f), 90, Digmask))//가로 오버렙박스
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
        */
    }

}
