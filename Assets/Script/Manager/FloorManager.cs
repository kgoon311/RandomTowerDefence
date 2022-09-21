using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorManager : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] Tilemap GrassTileMap;
    [SerializeField] Tilemap DigTileMap;
    [SerializeField] Tilemap FakeDirt;//ȭ�� �����θ� �ٷ� �� ��� �����ְ�
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
        Collider2D hit = Physics2D.OverlapBox(TargetFloor, new Vector2(0.9f, 2.9f), 0, Digmask);//���� �������ڽ�
        Collider2D hit2 = Physics2D.OverlapBox(TargetFloor, new Vector2(2.9f, 0.9f), 0, Digmask);//���� �������ڽ�
        if (hit == null && hit2 == null && Vector2.Distance(TargetFloor, RecentDigFloor) == 1)//�ٷ� ���� �ڽ��� ����� �ִ��� üũ
        {
            Vector3Int TilePos = GrassTileMap.WorldToCell(TargetFloor);//���콺 Ŭ�� ��ġ�� Ÿ�Ը� �������� �ٲ��ִ� �Լ�
            DiggingFloor.Add(TilePos);
            GrassTileMap.SetTile(TilePos, null);
            if (EndDigFloorPos != TargetFloor)
            {
                //�������ڽ��� �Ȱɸ��� ���� ������ ����ũŸ�ϸʿ� ����
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
        Collider2D V_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(0.9f, 2.9f), 0, Digmask);//���� �������ڽ�
        Collider2D H_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(2.9f, 0.9f), 0, Digmask);//���� �������ڽ�
        if (Glasshit && V_DigHit == null && H_DigHit == null && Vector2.Distance(mousePosition, RecentDigFloor) == 1)//���İ� ������ �ı� ����
        {
            M_Sprite.sprite = M_Images[0];
        }
        else if (DigEnd == true && Glasshit && !TurretHit && SelectTurret == true)//�ͷ� ��ġ�� ������ �Ϲ� ���� ������
        {
            M_Sprite.sprite = M_Images[0];
        }
        else if (DigEnd == true && SelectTurret == false && TurretHit)//�ͷ� ��ġ ������ �ƴҋ� �ͷ��� ���� �� ������
        { 
            M_Sprite.sprite = M_Images[0];
        }
        else
        {
            M_Sprite.sprite = M_Images[1];
        }
    }

}
