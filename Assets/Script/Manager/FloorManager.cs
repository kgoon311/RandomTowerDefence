using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorManager : Singleton<FloorManager>
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
    public List<Vector2> DiggingFloor = new List<Vector2>();

    [Header("MousePointer")]
    [SerializeField] GameObject m_Object;
    [SerializeField] Sprite[] m_Images = new Sprite[2];
    private SpriteRenderer m_Sprite;
    private Vector3 mousePosition;

    [Header("Turret")]
    [SerializeField] GameObject TurretBase;
    private bool selectTurret;
    private int selectTurretCost;
    
    private LayerMask grassmask;
    private LayerMask turretmask;
    private LayerMask digmask;


    void Start()
    {
        grassmask = LayerMask.GetMask("Floor");
        turretmask = LayerMask.GetMask("Turret");
        digmask = LayerMask.GetMask("DigFloor");
        m_Sprite = m_Object.GetComponent<SpriteRenderer>();
        ResetFloor();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(Mathf.Floor(mousePosition.x) + 0.5f, Mathf.Floor(mousePosition.y) + 0.5f);
        if (Input.GetKeyDown(KeyCode.R) && DigEnd == false) ResetFloor();
        if (Input.GetMouseButton(0)) ClickAction();
       
        MousePointerSpawn();
    }
    void ClickAction()
    {
        RaycastHit2D Glasshit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, grassmask);
        RaycastHit2D TurretHit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, turretmask);
        if (Glasshit && DigEnd == false)//잔디이고 땅파는 상황일 때
        {
            DigFloor(mousePosition);
        }
        else if (Glasshit && selectTurret == true)//잔디 이고 터렛 선택 했을때
        {
            TurretManager.Instance.AddScript(mousePosition, selectTurretCost);
            Debug.Log(mousePosition);
            GameManager.Instance.Monney -= selectTurretCost + 1;
            selectTurret = false;
        }
        else if (TurretHit)//터렛을 클릭했을때
        {
            TurretInformation(TurretHit.transform.gameObject);
        }
    }
    void ResetFloor()
    {
        RecentDigFloor = FirstDigFloorPos;

        Vector3Int TilePos;
        foreach (Vector2 Pos in DiggingFloor)
        {
            TilePos = GrassTileMap.WorldToCell(Pos);//Vector3Int로 변환
            GrassTileMap.SetTile(TilePos, Grass);//잔디 설치
            FakeDirt.SetTile(TilePos, Dirt);//가짜 흙 설치
            DigTileMap.SetTile(TilePos, null);//흙 지우기
        }

        DiggingFloor.Clear();
        DiggingFloor.Add(RecentDigFloor);//시작지점 할당 
        TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
        GrassTileMap.SetTile(TilePos, null);//시작지점 잔디 지우기
        FakeDirt.SetTile(TilePos, Dirt); //시작지점 가짜 흙 설치
    }
    void DigFloor(Vector2 TargetFloor)
    {
        Collider2D hit = Physics2D.OverlapBox(TargetFloor, new Vector2(0.9f, 2.9f), 0, digmask);//세로 오버랩박스
        Collider2D hit2 = Physics2D.OverlapBox(TargetFloor, new Vector2(2.9f, 0.9f), 0, digmask);//가로 오버렙박스
        if (hit == null && hit2 == null && Vector2.Distance(TargetFloor, RecentDigFloor) == 1)//바로 전에 자신의 블록이 있는지 체크
        {
            DiggingFloor.Add(TargetFloor);
            Vector3Int TilePos = GrassTileMap.WorldToCell(TargetFloor);//마우스 클릭 위치를 타입맵 기준으로 바꿔주는 함수
            GrassTileMap.SetTile(TilePos, null);
            if (EndDigFloorPos != TargetFloor) 
            {
                //오버랩박스에 안걸리기 위해 흙블록을 페이크타일맵에 적용
                FakeDirt.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                RecentDigFloor = TargetFloor;
            }//마지막 땅이 아닐때
            else
            {
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(TargetFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                DiggingFloor.Add(EndDigFloorPos + Vector2.up);//집 안으로 들어가기
                DigEnd = true;
            }
            GameManager.Instance.Monney += 1;
        }
    }
    public void BuildTurret(int SpawnRank)
    {
        selectTurret = true;
        selectTurretCost = SpawnRank;
    }
    void TurretInformation(GameObject Turret)
    {

    }
    void MousePointerSpawn()
    {
        m_Object.transform.position = mousePosition;
        RaycastHit2D Glasshit = Physics2D.Raycast(mousePosition, transform.forward, 10000f, grassmask);
        RaycastHit2D TurretHit = Physics2D.Raycast(mousePosition, transform.forward, 10000f, turretmask);
        Collider2D V_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(0.9f, 2.9f), 0, digmask);//세로 오버랩박스
        Collider2D H_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(2.9f, 0.9f), 0, digmask);//가로 오버렙박스
        if (Glasshit && V_DigHit == null && H_DigHit == null && Vector2.Distance(mousePosition, RecentDigFloor) == 1)//땅파고 있을떄 파기 가능
        {
            m_Sprite.sprite = m_Images[0];
        }
        else if (DigEnd == true && Glasshit && !TurretHit && selectTurret == true)//터렛 설치를 선택후 일반 땅에 있을떄
        {
            m_Sprite.sprite = m_Images[0];
        }
        else if (DigEnd == true && selectTurret == false && TurretHit)//터렛 설치 선택이 아닐떄 터렛을 누를 수 있을떄
        { 
            m_Sprite.sprite = m_Images[0];
        }
        else
        {
            m_Sprite.sprite = m_Images[1];
        }
    }

}
