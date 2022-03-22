using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    // Base Specialties
    public int gold = 2000;
    [SerializeField] [Range(1,10)] private int health = 100;
    private int currentHealth;
    
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;


    [SerializeField] private float spawnBaseOffset = 1.5f;
    // Soldiers
    [SerializeField] private GameObject knight;
    [SerializeField] private int knightCost = 100;

    private void Start()
    {
        currentHealth = health;
        healthText.text = currentHealth.ToString();
        
        
        Invoke("CreateSoldier",  UnityEngine.Random.Range(0f, 3f));
    }

    private void Update()
    {
        healthText.text = ((float)currentHealth / (float)health * 100).ToString() + '%';
        
    }


    private void CreateSoldier()
    {
        GameObject newSoldier = Instantiate(
            knight, 
            new Vector3(transform.position.x - spawnBaseOffset, knight.transform.position.y, 0), 
            Quaternion.Euler(0, 0, 0));
        newSoldier.tag = "enemy";
        newSoldier.transform.localScale = new Vector3(knight.transform.localScale.x*-1, knight.transform.localScale.y, 1);

        newSoldier.transform.parent = transform;

        gold -= knightCost;
        
        if(gold > knightCost)
        Invoke("CreateSoldier",  UnityEngine.Random.Range(3f, 13f));
    }
    
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = (float)currentHealth / (float)health;
        
        if (currentHealth <= 0)
        {
            Debug.Log("VICTORY!");
        }
    }
}
