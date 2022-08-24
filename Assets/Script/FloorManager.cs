using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject RecentDigFloor;
    private LayerMask mask;

    void Start()
    {
        mask = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("클릭");
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 100000, mask);
            Debug.Log(hit.transform.gameObject.name);
            if (hit)
            {
                DigFloor(hit.transform.gameObject);
            }
        }
    }
    void DigFloor(GameObject TargetFloor)
    {
        Debug.Log("실행");
        List<GameObject> Fourdirection = new List<GameObject>();
        bool Dig = false;
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor.transform.position, new Vector2(0.9f, 2.9f), 0, mask))
        {
            Fourdirection.Add(a.transform.gameObject);
        }
        foreach (Collider2D a in Physics2D.OverlapBoxAll(TargetFloor.transform.position, new Vector2(0.9f, 2.9f), 90, mask))
        {
            Fourdirection.Add(a.transform.gameObject);
        }

        int RecentIdxfloor = Fourdirection.IndexOf(RecentDigFloor);
        if (RecentIdxfloor > -1)
        {
            Debug.Log("전 블록 있음");

            Fourdirection.RemoveAt(RecentIdxfloor);
            Fourdirection.RemoveAll(a => a == TargetFloor);

            foreach (GameObject i in Fourdirection)
            {
                if (i.GetComponent<turret>().Diging)
                    Dig = true;
            }
            if (Dig == false)
            {
                TargetFloor.GetComponent<turret>().Diging = true;
                TargetFloor.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1);
                RecentDigFloor = TargetFloor;
            }
        }
    }

}
