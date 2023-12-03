using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private GameObject model;      // Model of the medkit
    [SerializeField] private GameObject marker;     // Medkit Marker
    [SerializeField] private Transform healSFX;     // Heal Sound

    // Medkit Settings
    [Header("Medkit Settings")]
    [SerializeField] private float minimumToHeal = 10f; // Minimum amount of damage to heal character
    [SerializeField] private float heal = 50f;          // Amount of heal the medkit do

    // Reference Variables
    private AudioSource healSFXSource;

    // Internal Variables
    private bool healed = false;

    // Constant Variables
    private const string PLAYERTAG = "Player";

    private void Awake()
    {
        healSFXSource = healSFX.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealthController targetHealthController = collision.GetComponent<HealthController>();
        if (targetHealthController)
        {
            // Heal player on contact if player has more than damage minimum
            if (collision.CompareTag(PLAYERTAG) && !healed)
            {
                if (targetHealthController.MaxHealth - targetHealthController.Health >= minimumToHeal)
                {
                    healed = true;
                    targetHealthController.Heal(heal);
                    float randomPitch = Random.Range(0.8f, 1.2f);
                    healSFXSource.pitch = randomPitch;
                    healSFXSource.Play();

                    model.SetActive(false);

                    Destroy(marker);
                    Destroy(gameObject, healSFXSource.clip.length * 1.5f);
                }
            }
        }
    }
}