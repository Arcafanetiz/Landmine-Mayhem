using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private Transform mainCharacter;   // The main character to target
    [SerializeField] private Transform enemyContainer;  // The container for enemy

    // Spawn Settings
    [Header("Enemy Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;        // Enemy prefab to use
    [SerializeField] private int maxEnemy = 10;             // Maximum enemy at a time
    [SerializeField] private float spawnDistance = 45;      // Spawn Distance of the enemy
    [SerializeField] private float spawnHeightOffset = 1;   // Spawn Height of the enemy

    // Internal Variables
    private int enemyCount;

    private void FixedUpdate()
    {
        enemyCount = enemyContainer.childCount;
    }

    public void SpawnEnemy()
    {
        // Check if maximum amount of enemy exists
        if (enemyCount < maxEnemy)
        {
            // Spawn location on the primary axis
            float spawnPrimary = Random.Range(-1f, 1f) > 0f ? -spawnDistance : spawnDistance;

            // Spawn location along the secondary axis
            float spawnSecondary = Random.Range(-spawnDistance, spawnDistance);

            // Choose axis as primary
            bool spawnXOrZ = Random.Range(0, 2) == 0;

            Vector3 spawnPosition;
            if (spawnXOrZ)
            {
                spawnPosition = new Vector3(spawnPrimary, spawnHeightOffset, spawnSecondary);
            }
            else
            {
                spawnPosition = new Vector3(spawnSecondary, spawnHeightOffset, spawnPrimary);
            }

            // Calculate the rotation needed to face the main character
            Vector3 directionToMainCharacter = mainCharacter.position - spawnPosition;
            Quaternion rotation = Quaternion.LookRotation(directionToMainCharacter);

            // Instantiate the enemy with the desired rotation and parent to the container
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, rotation);
            newEnemy.transform.parent = enemyContainer;
        }
    }
}