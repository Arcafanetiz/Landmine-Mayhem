using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;      //AI Movement Speed
    [SerializeField] private float turnSpeed = 360f;    //AI Turn Speed

    // Damage Settings
    [Header("Damage Settings")]
    [SerializeField] private float damage = 20f;            //AI Damage on touch
    [SerializeField] private float selfDamage = 10f;        //AI Take own damage on touch
    [SerializeField] private float selfKnockPower = 100f;    //AI Knock self back on touch

    // Reference Variables
    private Rigidbody enemyRigid;
    private GameObject targetToChase;

    // Constant Variables
    private const string PLAYERTAG = "Player";
    private void Awake()
    {
        enemyRigid = GetComponent<Rigidbody>();
        targetToChase = GameObject.FindWithTag(PLAYERTAG);
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
        //Use health controller system
        HealthController targetHealthController = collision.gameObject.GetComponent<HealthController>();
        if (targetHealthController)
        {
            //Damage player on contact, also take self damage and knock
            if (collision.gameObject.CompareTag(PLAYERTAG))
            {
                targetHealthController.Damage(damage);
                GetComponent<HealthController>().Damage(selfDamage);
                enemyRigid.AddForce(-transform.forward * selfKnockPower, ForceMode.Impulse);
            }
        } 
    }
}