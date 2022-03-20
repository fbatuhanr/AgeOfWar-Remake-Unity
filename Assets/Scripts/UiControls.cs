using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiControls : MonoBehaviour
{
    private GameObject playerBase;
    
    [SerializeField] private GameObject knight;
    [SerializeField] private float knightSpawnRate = 2.5f;

    private void Start()
    {
        playerBase = GameObject.FindWithTag("friendBase");
    }

    private float timeStamp = 0f;

    public void Spawn()
    {
        if (Time.time >= timeStamp)
        {
            Instantiate(
                knight, 
                new Vector3(playerBase.transform.position.x + 2.5f, knight.transform.position.y, 0), 
                Quaternion.Euler(0, 0, 0));
            
            timeStamp = Time.time + knightSpawnRate;
        }
    }
}
