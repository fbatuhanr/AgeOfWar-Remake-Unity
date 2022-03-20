using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    // Base Specialties
    [SerializeField] [Range(1,10)] private int health = 100;
    private int currentHealth;

    [SerializeField] private Image healthBar;

    private void Start()
    {
        currentHealth = health;
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
