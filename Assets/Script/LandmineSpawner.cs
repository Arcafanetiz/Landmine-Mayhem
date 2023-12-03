using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LandmineSpawner : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private GameObject landminePrefab; // Landmine Prefab
    [SerializeField] private Transform mainCharacter;   // Main Character
    [SerializeField] private Transform trapContainer;   // Container

    // Spawner Settings
    [Header("Landmine Spawner Settings")]
    [SerializeField] private float spawnThreshold = 3;  // How far apart should mine spawn as player move

    private Vector3 initialPlayerPosition;

    private void Start()
    {
        initialPlayerPosition = mainCharacter.position;
    }

    private void FixedUpdate()
    {
        if (mainCharacter)
        {
            //Spawn landmine after a distance has been reached
            float distanceMoved = Vector3.Distance(initialPlayerPosition, mainCharacter.position);

            if (distanceMoved >= spawnThreshold)
            {
                GameObject newLandmine = Instantiate(landminePrefab, mainCharacter.position, Quaternion.identity);
                newLandmine.transform.parent = trapContainer;
                initialPlayerPosition = mainCharacter.position;
            }
        }
    }
}