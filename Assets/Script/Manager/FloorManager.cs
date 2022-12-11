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
    [SerializeField] private bool isDigEnd;
    private Vector2 RecentDigFloor;
    public List<Vector2> DiggingFloor = new List<Vector2>();

    [Header("MousePointer")]
    [SerializeField] GameObject m_Object;
    [SerializeField] Sprite[] m_Images = new Sprite[2];
    private SpriteRenderer m_Sprite;
    private Vector3 mousePosition;

    [Header("Turret")]
    [SerializeField] GameObject TurretBase;
    private bool isSelectTurret;
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

        if (Input.GetKeyDown(KeyCode.R) && isDigEnd == false) ResetFloor();

        if (Input.GetMouseButtonDown(0)) L_ClickAction();//클릭했을때
        else if (Input.GetMouseButtonDown(2)) R_ClickAction();
        else if (Input.GetMouseButton(0)) ClickingAction();//클릭중일때
       
        MousePointerSpawn();
    }
    //클릭했을 때 
    void L_ClickAction()
    {
        RaycastHit2D TurretHit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, turretmask);//터렛 클릭 레이캐스트
        RaycastHit2D Glasshit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, grassmask);//잔디 클릭 레이캐스트

        if (TurretHit)//터렛을 클릭했을때
        {
            TurretInformation(TurretHit.transform.gameObject);
        }
        else if (Glasshit && isSelectTurret == true && isDigEnd)//잔디 이고 터렛설치 중일때
        {
            TurretManager.Instance.BuildTurret(mousePosition, selectTurretCost);

            GameManager.Instance._money -= selectTurretCost + 1;
            isSelectTurret = false;
        }
        else
        {
            isSelectTurret = false;
        }
    }
    //우클릭 했을 때
    void R_ClickAction()
    {
       if(isSelectTurret == true)
            isSelectTurret = false;
       //땅 설치중 한칸 취소구연 예전
    }
    //클릭중
    private void ClickingAction()
    {
        RaycastHit2D Glasshit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, grassmask);//잔디 클릭 레이캐스트

        if (Glasshit && isDigEnd == false)//잔디이고 땅파는 상황일 때
        {
            DigFloor(mousePosition);
        }
      
    }

    void ResetFloor()
    {
        Vector3Int TilePos;
        RecentDigFloor = FirstDigFloorPos;

        foreach (Vector2 Pos in DiggingFloor)
        {
            TilePos = GrassTileMap.WorldToCell(Pos);//Vector3Int로 변환

            GrassTileMap.SetTile(TilePos, Grass);//잔디 설치
            FakeDirt.SetTile(TilePos, null);//가짜 흙 지우기
            DigTileMap.SetTile(TilePos, null);//흙 지우기
        }

        TilePos = GrassTileMap.WorldToCell(RecentDigFloor);//다 초기화 후 처음 시작 흙 위치

        DiggingFloor.Clear();//초기화
        DiggingFloor.Add(RecentDigFloor);//시작지점 할당 


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
            if (EndDigFloorPos != TargetFloor) //마지막 땅이 아닐때
            {
                //오버랩박스에 안걸리기 위해 흙블록을 페이크타일맵에 적용
                FakeDirt.SetTile(TilePos, Dirt);
                
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);//이전 페이크 타일에 진짜 흙 타일 설치

                RecentDigFloor = TargetFloor;
            }
            else
            {
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(TargetFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                DiggingFloor.Add(EndDigFloorPos + Vector2.up);//집 안으로 들어가기
                isDigEnd = true;
            }
            GameManager.Instance._money += 1;
        }
    }

    //터렛 선택 버튼
    public void OnClickSelectTurret(int SpawnRank)
    {
        isSelectTurret = true;
        selectTurretCost = SpawnRank;
    }

    //터렛 정보 열기
    void TurretInformation(GameObject Turret)
    {

    }

    //마우스 위치 표시
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
        else if (isDigEnd == true && Glasshit && !TurretHit && isSelectTurret == true)//터렛 설치를 선택후 일반 땅에 있을떄
        {
            m_Sprite.sprite = m_Images[0];
        }
        else if (isDigEnd == true && isSelectTurret == false && TurretHit)//터렛 설치 선택이 아닐떄 터렛을 누를 수 있을떄
        { 
            m_Sprite.sprite = m_Images[0];
        }
        else
        {
            m_Sprite.sprite = m_Images[1];
        }
    }

}
