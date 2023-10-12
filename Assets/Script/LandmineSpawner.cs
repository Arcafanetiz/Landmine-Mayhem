using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LandmineSpawner : MonoBehaviour
{
    [Header("Attach GameObject")]
    [SerializeField] private GameObject landminePrefab;
    [SerializeField] private Transform mainCharacter;
    [SerializeField] private Transform trapHolder;
    [Header("Landmine Spawner Settings")]
    [SerializeField] private float spawnThreshold = 3;

    private Vector3 initialPlayerPosition;
    private void Start()
    {
        initialPlayerPosition = mainCharacter.position;
    }
    private void Update()
    {
        if (mainCharacter)
        {
            //Spawn landmine after a distance has been reached
            float distanceMoved = Vector3.Distance(initialPlayerPosition, mainCharacter.position);

            if (distanceMoved >= spawnThreshold)
            {
                GameObject newLandmine = Instantiate(landminePrefab, mainCharacter.position, Quaternion.identity);
                newLandmine.transform.parent = trapHolder;
                initialPlayerPosition = mainCharacter.position;
            }
        }
    }
}