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

    private void Start()
    {
        currentHealth = health;
        healthText.text = currentHealth.ToString();
    }

    private void Update()
    {
        healthText.text = ((float)currentHealth / (float)health * 100).ToString() + '%';
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
