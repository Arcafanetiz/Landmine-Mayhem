using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    // Properties
    public float MaxHealth
    {
        get => maxHealth;
        private set => maxHealth = value;
    }
    public float Health {
        get => health;
        private set => health = Mathf.Clamp(value, 0, MaxHealth);
    }

    // Internal Variables
    [Header("Health Value")]
    [SerializeField] private float health = 100f;    // Current health
    [SerializeField] private float maxHealth = 100f; // Maximum health

    private void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // External function to Heal
    public void Heal(float amount)
    {
        Health += amount;
    }

    // External function to Damage
    public void Damage(float amount)
    {
        Health -= amount;
    }
}