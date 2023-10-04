using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnYOffset;
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
        if (enemyCount < maxEnemy)
        {
            int randomIndex = Random.Range(1, enemySpawns.Length);
            Transform spawnPoint = enemySpawns[randomIndex];

            Vector3 spawnPosition = spawnPoint.position + Vector3.up * spawnYOffset;

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = enemyHolder.transform;
        }
    }
}
