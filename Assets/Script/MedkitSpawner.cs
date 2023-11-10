using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitSpawner : MonoBehaviour
{
    // Attach GameObject
    [Header("Attach GameObject")]
    [SerializeField] private Transform medkitContainer;

    // Spawner Settings
    [Header("Medkit Spawner Settings")]
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private float spawnYOffset = 10;
    [SerializeField] private float spawnXLimit = 10;
    [SerializeField] private float spawnZLimit = 10;

    public void SpawnMedkit()
    {
        // Get new spawn position
        Vector3 spawnPosition = new(UnityEngine.Random.Range(-spawnXLimit, spawnXLimit),
                                    spawnYOffset,
                                    UnityEngine.Random.Range(-spawnZLimit, spawnZLimit));

        // Rotate the medkit randomly
        Quaternion rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up);

        GameObject newEnemy = Instantiate(medkitPrefab, spawnPosition, rotation);
        newEnemy.transform.parent = medkitContainer;
    }
}
