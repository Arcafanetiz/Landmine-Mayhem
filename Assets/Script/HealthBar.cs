using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Attach GameObjects
    [Header("Attach GameObjects")]
    [SerializeField] private Transform healthSection; // The health bar
    [SerializeField] private Transform healthCap;     // The end of the health bar
    [SerializeField] private Transform healSection;   // The heal bar
    [SerializeField] private Transform healCap;       // The end of the heal bar
    [SerializeField] private Transform damageSection; // The damage bar

    // Animation Settings
    [Header("Animation Settings")]
    [SerializeField] private float animDuration = 1f; // Duration of the resolving animation
    [SerializeField] private float animDelay = 0.5f;  // Delay after taking damage before resolving animation

    // Billboard Settings
    [Header("Billboard Settings")]
    [SerializeField] private float heightOffset = 1f;    // The height offset of health bar

    // Reference Variables
    private Transform mainCamTransform;               // The transform of main camera
    private HealthController targetHealthController;  // Used to obtain health information
    private float MaxHealth => targetHealthController.MaxHealth; // Maximum health for percentage calculation
    private float Health => targetHealthController.Health;       // Current health for percentage calculation

    // Internal Variables
    private float perceivedHealth = 0f;  // The current health value used in calculations
    private float resolveDelaying = 0f;  // The current delay time before resolving the animation
    private float resolveTime = 0f;      // The current time for animation resolution

    private float percToResolveDamage = 0f; // The percentage of damage bar before resolving the animation
    private float percToResolveHeal = 0f;   // The percentage of the heal bar before resolving the animation

    private void Awake()
    {
        mainCamTransform = Camera.main.transform;
    }
    private void Start()
    {
        // Setup the health bar, disable if health controller not found
        // Health controller is always fetched from the parent of health bar
        targetHealthController = transform.parent.GetComponent<HealthController>();
        if (targetHealthController)
        {
            perceivedHealth = Health;
        }
        else
        {
            gameObject.SetActive(false);
        }
        healSection.localScale = new Vector3(0, 1, 1);
        damageSection.localScale = new Vector3(0, 1, 1);
    }
    private void Update()
    {
        if (targetHealthController)
        {
            float newHealth = Health;
            //Only active when health changes
            if (perceivedHealth != newHealth)
            {
                float changedHealth = newHealth - perceivedHealth;
                float percNewHealth = GetPercentage(newHealth);
                float percChangedHealth = GetPercentage(changedHealth);

                // Handle healing
                if (changedHealth > 0)
                {
                    // Add to the heal bar
                    percToResolveHeal += percChangedHealth;
                    SetHealBar();

                    // Reduce the damage bar
                    percToResolveDamage = Mathf.Max(0, percToResolveDamage - percChangedHealth);
                    SetDamageBar();
                }
                //Handle damage
                else if (changedHealth < 0)
                {
                    // If there's a heal bar, reduce it first
                    if (percToResolveHeal > 0)
                    {
                        percToResolveHeal += percChangedHealth;

                        // Bleeds into real health bar when heal bar runs out
                        if (percToResolveHeal < 0)
                        {
                            percToResolveHeal = 0;
                            healthSection.localScale = new Vector3(percNewHealth, 1, 1);
                        }
                        SetHealBar();
                    }
                    // Without a heal bar, reduce the health bar
                    else
                    {
                        healthSection.localScale = new Vector3(percNewHealth, 1, 1);
                    }
                    healSection.position = healthCap.position;

                    // Add to the damage bar
                    percToResolveDamage -= percChangedHealth;
                    SetDamageBar();
                }
                //Update perceived health and refresh the delay
                perceivedHealth = newHealth;
                resolveDelaying = animDelay;
                resolveTime = 0;
            }
            // Check the delay timer; when it reaches zero, tick health bar animation
            if (resolveDelaying > 0)
            {
                resolveDelaying = Mathf.Max(0, resolveDelaying -= Time.deltaTime);
            }
            else
            {
                TickResolveHealthBar();
            }
        }
    }
    private void LateUpdate()
    {
        //Billboard effect
        transform.SetPositionAndRotation(
            transform.parent.position + mainCamTransform.rotation * new Vector3(0, heightOffset, 0),
            mainCamTransform.rotation
            );
    }
    private void TickResolveHealthBar()
    {
        //Resolving the health bar animation using lerp and duration for synced bar movement
        resolveTime = Mathf.Min(resolveTime + (Time.deltaTime / animDuration), 1);
        if (percToResolveDamage > 0)
        {
            percToResolveDamage = Mathf.Lerp(percToResolveDamage, 0, resolveTime);
            SetDamageBar();
        }
        if (percToResolveHeal > 0)
        {
            percToResolveHeal = Mathf.Lerp(percToResolveHeal, 0, resolveTime);
            healthSection.localScale = new Vector3(GetPercentage(Health) - percToResolveHeal, 1, 1);
            SetHealBar();
        }
    }
    private float GetPercentage(float amount)
    {
        return (amount / MaxHealth);
    }
    //Function to refresh heal bar position/size
    private void SetHealBar()
    {
        healSection.position = healthCap.position;
        healSection.localScale = new Vector3(percToResolveHeal, 1, 1);
    }
    //Function to refresh damage bar position/size
    private void SetDamageBar()
    {
        damageSection.position = healCap.position;
        damageSection.localScale = new Vector3(percToResolveDamage, 1, 1);
    }
}