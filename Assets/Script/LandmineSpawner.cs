using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LandmineSpawner : MonoBehaviour
{
    public GameObject landminePrefab;
    public float spawnDistanceThreshold;

    private Vector3 initialPlayerPosition;
    private GameObject trapHolder;
    private Transform character;
    void Start()
    {
        character = GameObject.Find("Character").transform;
        initialPlayerPosition = character.position;
        trapHolder = GameObject.Find("TrapHolder");
    }
    void Update()
    {
        if (character)
        {
            float distanceMoved = Vector3.Distance(initialPlayerPosition, character.position);

            if (distanceMoved >= spawnDistanceThreshold)
            {
                GameObject newLandmine = Instantiate(landminePrefab, character.position, Quaternion.identity);
                newLandmine.transform.parent = trapHolder.transform;
                initialPlayerPosition = character.position;
            }
        }
    }
}