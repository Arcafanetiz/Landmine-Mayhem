using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    // Attach GameObject
    [Header("Attach GameObject")]
    [SerializeField] private Transform hitSFX;

    // Landmine Settings
    [Header("Landmine Settings")]
    [SerializeField] private float armingDistance = 2f; //Distance moved before landmine arms itself
    [SerializeField] private float damage = 20f;        //Amount of damage the mine do

    // Reference Variables
    private GameObject mainCharacter;
    private AudioSource hitSFXSource;

    // Internal Variables
    private bool landmineArmed = false;

    // Constant Variables
    private const string PLAYERTAG = "Player";
    private const string ENEMYTAG = "Enemy";

    private void Awake()
    {
        mainCharacter = GameObject.FindWithTag(PLAYERTAG);
        hitSFXSource = hitSFX.GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (!landmineArmed)
        {
            //Arm landmine when player is out of distance
            if (Vector3.Distance(mainCharacter.transform.position, transform.position) > armingDistance)
            {
                landmineArmed = true;
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (landmineArmed)
        {
            HealthController targetHealthController = collision.gameObject.GetComponent<HealthController>();
            if (targetHealthController)
            {
                //Damage player or enemy on contact
                if (collision.gameObject.CompareTag(PLAYERTAG) || collision.gameObject.CompareTag(ENEMYTAG))
                {
                    targetHealthController.Damage(damage);
                    hitSFX.parent = transform.parent;
                    hitSFXSource.time = 0.14f;
                    hitSFXSource.Play();
                    Destroy(gameObject);
                }
            }
        }
    }
}