using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Landmine : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private GameObject model;      // Model of the mine
    [SerializeField] private GameObject beep;       // Light part of the mine
    [SerializeField] private Transform hitSFX;      // Hit Sound
    [SerializeField] private VisualEffect hitVFX;   // Hit Effect

    // Landmine Settings
    [Header("Landmine Settings")]
    [SerializeField] private float armingDistance = 2f; //Distance moved before landmine arms itself
    [SerializeField] private float damage = 20f;        //Amount of damage the mine do

    // Reference Variables
    private Transform mainCharacter;
    private AudioSource hitSFXSource;

    // Internal Variables
    private bool landmineArmed = false;
    private bool landmineExploded = false;

    // Constant Variables
    private const string PLAYERTAG = "Player";
    private const string ENEMYTAG = "Enemy";

    private void Awake()
    {
        mainCharacter = GameObject.FindWithTag(PLAYERTAG).transform;
        hitSFXSource = hitSFX.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!landmineArmed)
        {
            //Arm landmine when player is out of distance
            if (Vector3.Distance(mainCharacter.position, transform.position) > armingDistance)
            {
                landmineArmed = true;
                beep.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (landmineArmed && !landmineExploded)
        {
            HealthController targetHealthController = collision.GetComponent<HealthController>();
            if (targetHealthController)
            {
                //Damage player or enemy on contact
                if (collision.CompareTag(PLAYERTAG) || collision.CompareTag(ENEMYTAG))
                {
                    landmineExploded = true;
                    targetHealthController.Damage(damage);
                    float randomPitch = Random.Range(0.8f, 1.2f);
                    hitSFXSource.pitch = randomPitch;
                    hitSFXSource.Play();
                    hitVFX.Play();

                    model.SetActive(false);
                    beep.SetActive(false);

                    Destroy(gameObject, hitSFXSource.clip.length * 1.5f);
                }
            }
        }
    }
}