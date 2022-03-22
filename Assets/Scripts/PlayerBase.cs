using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    // Base Specialties
    public int gold = 2000;
    [SerializeField] [Range(1,10)] private int health = 100;
    private int currentHealth;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Image healthBar;

    private void Start()
    {
        currentHealth = health;
        
        healthText.text = currentHealth.ToString();
        goldText.text = "Gold: " + gold;
    }

    private void Update()
    {
        healthText.text = ((float)currentHealth / (float)health * 100).ToString() + '%';
        // Debug.Log("current health: " + currentHealth + " health: " + health + " islem: " + ((float)currentHealth / health * 100));
        goldText.text = "Gold: " + gold;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = (float)currentHealth / (float)health;
        
        if (currentHealth <= 0)
        {
            Debug.Log("GAME OVER!");
        }
    }
}
