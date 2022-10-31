using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorManager : Singleton<FloorManager>
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
        if (Glasshit && DigEnd == false)//�ܵ��̰� ���Ĵ� ��Ȳ�� ��
        {
            DigFloor(mousePosition);
        }
        else if (Glasshit && selectTurret == true)//�ܵ� �̰� �ͷ� ���� ������
        {
            TurretManager.Instance.AddScript(mousePosition, selectTurretCost);
            Debug.Log(mousePosition);
            GameManager.Instance.Monney -= selectTurretCost + 1;
            selectTurret = false;
        }
        else if (TurretHit)//�ͷ��� Ŭ��������
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
            TilePos = GrassTileMap.WorldToCell(Pos);//Vector3Int�� ��ȯ
            GrassTileMap.SetTile(TilePos, Grass);//�ܵ� ��ġ
            FakeDirt.SetTile(TilePos, Dirt);//��¥ �� ��ġ
            DigTileMap.SetTile(TilePos, null);//�� �����
        }

        DiggingFloor.Clear();
        DiggingFloor.Add(RecentDigFloor);//�������� �Ҵ� 
        TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
        GrassTileMap.SetTile(TilePos, null);//�������� �ܵ� �����
        FakeDirt.SetTile(TilePos, Dirt); //�������� ��¥ �� ��ġ
    }
    void DigFloor(Vector2 TargetFloor)
    {
        Collider2D hit = Physics2D.OverlapBox(TargetFloor, new Vector2(0.9f, 2.9f), 0, digmask);//���� �������ڽ�
        Collider2D hit2 = Physics2D.OverlapBox(TargetFloor, new Vector2(2.9f, 0.9f), 0, digmask);//���� �������ڽ�
        if (hit == null && hit2 == null && Vector2.Distance(TargetFloor, RecentDigFloor) == 1)//�ٷ� ���� �ڽ��� ����� �ִ��� üũ
        {
            DiggingFloor.Add(TargetFloor);
            Vector3Int TilePos = GrassTileMap.WorldToCell(TargetFloor);//���콺 Ŭ�� ��ġ�� Ÿ�Ը� �������� �ٲ��ִ� �Լ�
            GrassTileMap.SetTile(TilePos, null);
            if (EndDigFloorPos != TargetFloor) 
            {
                //�������ڽ��� �Ȱɸ��� ���� ������ ����ũŸ�ϸʿ� ����
                FakeDirt.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                RecentDigFloor = TargetFloor;
            }//������ ���� �ƴҶ�
            else
            {
                TilePos = GrassTileMap.WorldToCell(RecentDigFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                TilePos = GrassTileMap.WorldToCell(TargetFloor);
                DigTileMap.SetTile(TilePos, Dirt);
                DiggingFloor.Add(EndDigFloorPos + Vector2.up);//�� ������ ����
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
        Collider2D V_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(0.9f, 2.9f), 0, digmask);//���� �������ڽ�
        Collider2D H_DigHit = Physics2D.OverlapBox(mousePosition, new Vector2(2.9f, 0.9f), 0, digmask);//���� �������ڽ�
        if (Glasshit && V_DigHit == null && H_DigHit == null && Vector2.Distance(mousePosition, RecentDigFloor) == 1)//���İ� ������ �ı� ����
        {
            m_Sprite.sprite = m_Images[0];
        }
        else if (DigEnd == true && Glasshit && !TurretHit && selectTurret == true)//�ͷ� ��ġ�� ������ �Ϲ� ���� ������
        {
            m_Sprite.sprite = m_Images[0];
        }
        else if (DigEnd == true && selectTurret == false && TurretHit)//�ͷ� ��ġ ������ �ƴҋ� �ͷ��� ���� �� ������
        { 
            m_Sprite.sprite = m_Images[0];
        }
        else
        {
            m_Sprite.sprite = m_Images[1];
        }
    }

}
