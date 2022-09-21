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
    [Header("MousePointer")]
    [SerializeField] GameObject M_Object;
    [SerializeField] Sprite[] M_Images = new Sprite[2];
    private SpriteRenderer M_Sprite;
    private Vector3 mousePosition;
    [Header("Turret")]
    [SerializeField] GameObject umm;
    private bool SelectTurret;
    private int SelectTurretCost;
    
    private LayerMask Grassmask;
    private LayerMask turretmask;
    private LayerMask Digmask;


    void Start()
    {
        Grassmask = LayerMask.GetMask("Floor");
        turretmask = LayerMask.GetMask("Turret");
        Digmask = LayerMask.GetMask("DigFloor");
        M_Sprite = M_Object.GetComponent<SpriteRenderer>();
        ResetFloor();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(Mathf.Floor(mousePosition.x) + 0.5f, Mathf.Floor(mousePosition.y) + 0.5f);
        if (Input.GetKeyDown(KeyCode.R) && DigEnd == false)
        {
            ResetFloor();
        }
        if (Input.GetMouseButtonDown(0) )
        {
            RaycastHit2D Glasshit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, Grassmask);
            RaycastHit2D TurretHit = Physics2D.Raycast(mousePosition, transform.forward, 100000f, turretmask);
            if (Glasshit && DigEnd == false)
            {
                DigFloor(mousePosition);
            }
            else if(Glasshit && SelectTurret == true)
            {
                TurretManager.instance.AddScript(mousePosition, SelectTurretCost);
                Debug.Log(mousePosition);
                GameManager.instance.Monney -= SelectTurretCost+1;
                SelectTurret = false;
            }
            else if(TurretHit)
            { 
                TurretInformation(TurretHit.transform.gameObject);
            }
        }
       
        MousePointerSpawn();
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
        Collider2D hit = Physics2D.OverlapBox(TargetFloor, new Vector2(0.9f, 2.9f), 0, Digmask);//세로 오버랩박스
        Collider2D hit2 = Physics2D.OverlapBox(TargetFloor, new Vector2(2.9f, 0.9f), 0, Digmask);//가로 오버렙박스
        if (hit == null && hit2 == null && Vector2.Distance(TargetFloor, RecentDigFloor) == 1)//바로 전에 자신의 블록이 있는지 체크
        {
            Vector3Int TilePos = GrassTileMap.WorldToCell(TargetFloor);//마우스 클릭 위치를 타입맵 기준으로 바꿔주는 함수
            DiggingFloor.Add(TilePos);
            GrassTileMap.SetTile(TilePos, null);
            if (EndDigFloorPos != TargetFloor)
            {
                //오버랩박스에 안걸리기 위해 흙블록을 페이크타일맵에 적용
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
            GameManager.instance.Monney += 1;
        }
    }
    public void BuildTurret(int SpawnRank)
    {
        SelectTurret = true;
        SelectTurretCost = SpawnRank;
    }
    void TurretInformation(GameObject Turret)
    {

    }
    void MousePointerSpawn()
    {
        M_Object.transform.position = mousePosition;
        RaycastHit2D Glasshit = Physics2D.Raycast(mousePosition, transform.forward, 10000f, Grassmask);
        RaycastHit2D TurretHit = Physics2D.Raycast(mousePosition, transform.forward, 10000f, turretmask);
        Collider2D V_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(0.9f, 2.9f), 0, Digmask);//세로 오버랩박스
        Collider2D H_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(2.9f, 0.9f), 0, Digmask);//가로 오버렙박스
        if (Glasshit && V_DigHit == null && H_DigHit == null && Vector2.Distance(mousePosition, RecentDigFloor) == 1)//땅파고 있을떄 파기 가능
        {
            M_Sprite.sprite = M_Images[0];
        }
        else if (DigEnd == true && Glasshit && !TurretHit && SelectTurret == true)//터렛 설치를 선택후 일반 땅에 있을떄
        {
            M_Sprite.sprite = M_Images[0];
        }
        else if (DigEnd == true && SelectTurret == false && TurretHit)//터렛 설치 선택이 아닐떄 터렛을 누를 수 있을떄
        { 
            M_Sprite.sprite = M_Images[0];
        }
        else
        {
            M_Sprite.sprite = M_Images[1];
        }
    }

}
