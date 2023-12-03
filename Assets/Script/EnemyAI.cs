using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private Animator animator; // Enemy Animator

    // Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;      // Enemy Movement Speed
    [SerializeField] private float turnSpeed = 360f;    // Enemy Turn Speed

    // Damage Settings
    [Header("Damage Settings")]
    [SerializeField] private float damage = 20f;            // Damage enemy do
    [SerializeField] private float selfDamage = 10f;        // Damage enemy take
    [SerializeField] private float selfKnockPower = 100f;   // Knockback enemy take

    // Reference Variables
    private Rigidbody enemyRigid;
    private GameObject targetToChase;
    private AudioSource hurtSFXSource;

    // Constant Variables
    private const string PLAYERTAG = "Player";
    private const string ANIMATION_VELOCITY = "Velocity";

    private void Awake()
    {
        enemyRigid = GetComponent<Rigidbody>();
        targetToChase = GameObject.FindWithTag(PLAYERTAG);
        hurtSFXSource = targetToChase.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        moveSpeed = GameManager.EnemySpeed;
        if (targetToChase)
        {
            //Get Direction and move enemy towards character
            Vector3 direction = (targetToChase.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
            enemyRigid.velocity = transform.forward * moveSpeed;
            animator.SetFloat(ANIMATION_VELOCITY, enemyRigid.velocity.magnitude);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Use health controller system
        HealthController targetHealthController = collision.GetComponent<HealthController>();
        if (targetHealthController)
        {
            //Damage player on contact, also take self damage and knock
            if (collision.CompareTag(PLAYERTAG))
            {
                targetHealthController.Damage(damage);
                GetComponent<HealthController>().Damage(selfDamage);
                enemyRigid.AddForce(-transform.forward * selfKnockPower, ForceMode.Impulse);
                float randomPitch = Random.Range(0.8f, 1.2f);
                hurtSFXSource.pitch = randomPitch;
                hurtSFXSource.time = 0.1f;
                hurtSFXSource.Play();
            }
        } 
    }
}