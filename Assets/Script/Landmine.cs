using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    // Landmine Settings
    [Header("Landmine Settings")]
    [SerializeField] private float armingDistance = 2f; //Distance moved before landmine arms itself
    [SerializeField] private float damage = 20f;        //Amount of damage the mine do

    // Reference Variables
    private GameObject mainCharacter;

    // Internal Variables
    private bool landmineArmed = false;

    // Constant Variables
    private const string PLAYERTAG = "Player";
    private const string ENEMYTAG = "Enemy";

    private void Awake()
    {
        mainCharacter = GameObject.FindWithTag(PLAYERTAG);
    }
    private void Update()
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
                //Damage player on contact
                if (collision.gameObject.CompareTag(PLAYERTAG))
                {
                    targetHealthController.Damage(damage);
                    Destroy(gameObject);
                }
                //Damage enemy on contact
                if (collision.gameObject.CompareTag(ENEMYTAG))
                {
                    targetHealthController.Damage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}