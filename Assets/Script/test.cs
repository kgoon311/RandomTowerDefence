using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] GameObject testEnemy;
    [SerializeField] Vector3 spawnPos;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(testEnemy, spawnPos, transform.rotation);
        }
    }
}
