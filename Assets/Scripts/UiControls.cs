using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiControls : MonoBehaviour
{
    private GameObject playerBase;
    
    [SerializeField] private GameObject knight;
    [SerializeField] private float knightSpawnRate = 2.5f;
    [SerializeField] private int knightCost = 100;

    private Image knightImage;
    private TextMeshProUGUI knightCostText;

    private void Start()
    {
        playerBase = GameObject.FindWithTag("friendBase");

        knightCostText = transform.Find("KnightSpawner/KnightCost").GetComponent<TextMeshProUGUI>();
        knightCostText.text = knightCost.ToString();

        knightImage = transform.Find("KnightSpawner").GetComponent<Image>();
    }

    private float timeStamp = 0f;

    public void Spawn()
    {
        if (Time.time >= timeStamp && playerBase.GetComponent<PlayerBase>().gold >= knightCost)
        {
            GameObject newSpawn = Instantiate(
                knight, 
                new Vector3(playerBase.transform.position.x + 1.5f, knight.transform.position.y, 0), 
                Quaternion.Euler(0, 0, 0));
            newSpawn.transform.parent = playerBase.transform;

            playerBase.GetComponent<PlayerBase>().gold -= knightCost;
            
            timeStamp = Time.time + knightSpawnRate;
        }
    }

    private void Update() 
    {
        Debug.Log(Time.time - timeStamp < 0 ? (Time.time - timeStamp)*-1 < 0.5f ?  : "");
        // knightImage.fillAmount = -1/(Time.time - timeStamp) > 0 ? -1/(Time.time - timeStamp) : knightImage.fillAmount;
    }
}
