using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [Header("Landmine Settings")]
    public float armingDistance;

    private bool landmineArmed = false;
    private Transform player;
    void Start()
    {
        player = GameObject.Find("Character").transform;
    }
    void Update()
    {
        if (!landmineArmed)
        {
            //Arm landmine when player is out of distance
            if (Vector3.Distance(player.position, transform.position) > armingDistance)
            {
                landmineArmed = true;
                transform.Find("Beep").GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (landmineArmed)
        {
            //Kill player on contact
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            //Kill enemy on contact
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}