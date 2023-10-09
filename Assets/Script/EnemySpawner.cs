using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Y Offset Settings")]
    public float spawnYOffset;
    [Header("Enemy Spawn Settings")]
    public int maxEnemy;
    public GameObject enemyPrefab;

    private Transform[] enemySpawns;
    private int enemyCount;

    private GameObject enemyHolder;
    void Start()
    {
        enemySpawns = GameObject.Find("SpawnSet").GetComponentsInChildren<Transform>();
        enemyHolder = GameObject.Find("EnemyHolder");
    }
    void Update()
    {
        enemyCount = enemyHolder.transform.childCount;
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
            newEnemy.transform.parent = enemyHolder.transform;
        }
    }
}