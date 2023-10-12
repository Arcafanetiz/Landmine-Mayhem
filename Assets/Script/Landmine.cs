using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [Header("Landmine Settings")]
    [SerializeField] private float armingDistance;

    private bool landmineArmed = false;
    private GameObject mainCharacter;

    private const string PlayerTag = "Player";
    private const string EnemyTag = "Enemy";
    private const int Zero = 0;
    private void Start()
    {
        mainCharacter = GameObject.FindWithTag(PlayerTag);
    }
    private void Update()
    {
        if (!landmineArmed)
        {
            //Arm landmine when player is out of distance
            if (Vector3.Distance(mainCharacter.transform.position, transform.position) > armingDistance)
            {
                landmineArmed = true;
                transform.GetChild(Zero).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (landmineArmed)
        {
            //Kill player on contact
            if (collision.gameObject.CompareTag(PlayerTag))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            //Kill enemy on contact
            if (collision.gameObject.CompareTag(EnemyTag))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}