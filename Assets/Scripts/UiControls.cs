using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiControls : MonoBehaviour
{
    private GameObject playerBase;
    
    [SerializeField] private GameObject knight;
    [SerializeField] private float knightSpawnRate = 2.5f;
    [SerializeField] private int knightCost = 100;

    private TextMeshProUGUI knightCostText;

    private void Start()
    {
        playerBase = GameObject.FindWithTag("friendBase");

        knightCostText = transform.Find("KnightSpawner/KnightCost").GetComponent<TextMeshProUGUI>();
        knightCostText.text = knightCost.ToString();
    }

    private float timeStamp = 0f;

    public void Spawn()
    {
        if (Time.time >= timeStamp && playerBase.GetComponent<PlayerBase>().gold >= knightCost)
        {
            Instantiate(
                knight, 
                new Vector3(playerBase.transform.position.x + 1f, knight.transform.position.y, 0), 
                Quaternion.Euler(0, 0, 0));

            playerBase.GetComponent<PlayerBase>().gold -= knightCost;
            
            timeStamp = Time.time + knightSpawnRate;
        }
    }
}
