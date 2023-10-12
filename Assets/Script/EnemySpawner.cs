using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Attach GameObject")]
    [SerializeField] private Transform enemyHolder;
    [SerializeField] private Transform spawnSet;
    [Header("Spawn Y Offset Settings")]
    [SerializeField] private float spawnYOffset = 1;
    [Header("Enemy Spawn Settings")]
    [SerializeField] private int maxEnemy;
    [SerializeField] private GameObject enemyPrefab;

    private Transform[] enemySpawns;
    private int enemyCount;

    private void Start()
    {
        enemySpawns = spawnSet.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        enemyCount = enemyHolder.childCount;
    }
    public void SpawnEnemy()
    {
        //Check if maximum amount of enemy exists
        if (enemyCount < maxEnemy)
        {
            //Choose random spawn
            int randomIndex = Random.Range(1, enemySpawns.Length);
            Transform spawnPoint = enemySpawns[randomIndex];
            //Calculate spawn position
            Vector3 spawnPosition = spawnPoint.position + Vector3.up * spawnYOffset;
            //Spawn enemy and parent to holder
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = enemyHolder;
        }
    }
}