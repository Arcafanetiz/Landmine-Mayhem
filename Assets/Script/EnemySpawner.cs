using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Attach GameObject")]
    [SerializeField] private Transform mainCharacter;
    [SerializeField] private Transform enemyHolder;
    [SerializeField] private Transform spawnSet;

    [Header("Spawn Y Offset Settings")]
    [SerializeField] private float spawnYOffset = 1;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private int maxEnemy = 10;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minimumDistance = 3;

    private Transform[] enemySpawns;
    private int enemyCount;

    private void Awake()
    {
        enemySpawns = spawnSet.GetComponentsInChildren<Transform>();
    }
    private void FixedUpdate()
    {
        enemyCount = enemyHolder.childCount;
    }
    public void SpawnEnemy()
    {
        // Check if maximum amount of enemy exists
        if (enemyCount < maxEnemy)
        {
            // Retry 3 times, fails when spawn is too close
            Boolean spawned = false;
            int tries = 0;
            while (!spawned && tries < 3)
            {
                tries++;
                // Choose random spawn
                int randomIndex = UnityEngine.Random.Range(1, enemySpawns.Length);
                Transform spawnPoint = enemySpawns[randomIndex];
                // Try new spawn if distance is too close
                if ((spawnPoint.position - mainCharacter.position).magnitude > minimumDistance)
                {
                    // Calculate spawn position
                    Vector3 spawnPosition = spawnPoint.position + Vector3.up * spawnYOffset;
                    // Calculate the direction from the enemy to the main character
                    Vector3 directionToMainCharacter = mainCharacter.position - spawnPosition;
                    // Calculate the rotation needed to face the main character
                    Quaternion rotation = Quaternion.LookRotation(directionToMainCharacter);

                    // Instantiate the enemy with the desired rotation and parent to the holder
                    GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, rotation);
                    newEnemy.transform.parent = enemyHolder;

                    spawned = true;
                }
            }
        }
    }
}