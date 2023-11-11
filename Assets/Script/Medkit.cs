using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    // Medkit Settings
    [Header("Medkit Settings")]
    [SerializeField] private float minimumToHeal = 10f; // Minimum amount of damage to heal character
    [SerializeField] private float heal = 50f;          // Amount of heal the medkit do

    // Internal Variables
    private bool healed = false;

    // Constant Variables
    private const string PLAYERTAG = "Player";

    private void OnTriggerEnter(Collider collision)
    {
        HealthController targetHealthController = collision.gameObject.GetComponent<HealthController>();
        if (targetHealthController)
        {
            // Heal player on contact if player has more than damage minimum
            if (collision.gameObject.CompareTag(PLAYERTAG) && !healed)
            {
                if (targetHealthController.MaxHealth - targetHealthController.Health >= minimumToHeal)
                {
                    healed = true;
                    targetHealthController.Heal(heal);
                    Destroy(gameObject);
                }
            }
        }
    }
}