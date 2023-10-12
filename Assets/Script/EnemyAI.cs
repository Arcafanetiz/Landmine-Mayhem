using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    private Rigidbody enemyRigid;
    private GameObject targetToChase;

    private const string PlayerTag = "Player";
    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody>();
        targetToChase = GameObject.FindWithTag(PlayerTag);
    }
    private void Update()
    {
        moveSpeed = GameManager.EnemySpeed;
        if (targetToChase)
        {
            //Get Direction and move enemy towards character
            Vector3 direction = (targetToChase.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            enemyRigid.velocity = transform.forward * moveSpeed;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //Kill player upon contact
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}