using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private Transform targetToFollow;      // Target to Follow

    // Layer Variables
    [Header("Set Layer")]
    [SerializeField] private LayerMask obstacleLayerMask;   // Layer of obstacles the camera is obstructed by

    // Turn Speed Settings
    [Header("Turn Speed Settings")]
    [SerializeField] private float speedX = 500f;   // Turn speed aka. Sensitivity
    [SerializeField] private float speedY = 500f;   // Turn speed aka. Sensitivity

    // Pitch Settings
    [Header("Pitch Settings")]
    [SerializeField] private float minY = 25f;  // Minimum Pitch Up
    [SerializeField] private float maxY = 60f;  // Maximum Pitch Down

    // Orbit Settings
    [Header("Orbit Distance Settings")]
    [SerializeField] private float orbit = 10f;             // Orbit Distance
    [SerializeField] private float minOrbit = 0.6f;         // Orbit Minimum when too close
    [SerializeField] private float maxOrbitToCheck = 10f;   // Distance to check for obstruction
    [SerializeField] private float nearClipPlane = 0.3f;    // How far from the obstruction to hand towards

    // Internal Variables
    private float currentXAngle;
    private float currentYAngle;
    private RaycastHit obstacleHit;

    // Constant Variables
    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";
    private const float FullCircle = 360f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (targetToFollow)
        {
            // Get Camera Angle by X Axis
            currentXAngle += Input.GetAxis(MouseXAxisName) * speedX * Time.deltaTime;
            currentXAngle = (currentXAngle + FullCircle) % FullCircle;

            // Get Camera Angle by Y Axis, Opposite Direction
            currentYAngle += Input.GetAxis(MouseYAxisName) * speedY * Time.deltaTime * -1;
            currentYAngle = Mathf.Clamp(currentYAngle, minY, maxY);

            // Turn Camera to Result Angle, no Roll
            transform.rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0);

            // Check Obstruction
            float resultOrbit = orbit;
            if (Physics.Raycast(targetToFollow.position, -transform.forward, out obstacleHit, maxOrbitToCheck, obstacleLayerMask))
            {
                resultOrbit = Mathf.Max(obstacleHit.distance - nearClipPlane, minOrbit);
            }
            // Perform Orbit Distance
            transform.position = targetToFollow.position - (transform.forward * resultOrbit);
        }
    }
}