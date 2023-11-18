using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class RiverSound : MonoBehaviour
{
    [Header("Attach GameObject")]
    [SerializeField] private Transform riverParent;

    // Update is called once per frame
    void Update()
    {
        float closestDistance = 1000;
        Vector3 closestPosition = Vector3.zero;
        foreach (Transform child in riverParent)
        {
            Vector3 targetPosition = child.GetComponent<Collider>().ClosestPoint(Camera.main.transform.position);
            float targetDistance = (targetPosition - Camera.main.transform.position).magnitude;
            if (targetDistance < closestDistance)
            {
                closestDistance = targetDistance;
                closestPosition = targetPosition;
            }
        }
        transform.position = new Vector3(closestPosition.x, 0, closestPosition.z);
    }
}
