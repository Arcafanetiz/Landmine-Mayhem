using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
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
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}