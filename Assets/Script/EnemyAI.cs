using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float turnSpeed;

    private Rigidbody rigid;
    private GameObject target;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigid = GetComponent<Rigidbody>();
        target = GameObject.Find("Character");
    }
    void Update()
    {
        moveSpeed = gameManager.enemySpeed;
        if (target != null)
        {
            //Get Direction and move enemy towards character
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            rigid.velocity = transform.forward * moveSpeed;
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        //Kill player upon contact
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}