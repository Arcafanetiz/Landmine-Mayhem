using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Attach GameObject")]
    [SerializeField] private Transform targetToFollow;
    [Header("Set Layer")]
    [SerializeField] private LayerMask obstacleLayerMask;
    [Header("Turn Speed Settings")]
    [SerializeField] private float speedX = 500f;
    [SerializeField] private float speedY = 500f;
    [Header("Pitch Settings")]
    [SerializeField] private float minY = 25f;
    [SerializeField] private float maxY = 60f;
    [Header("Orbit Distance Settings")]
    [SerializeField] private float orbit = 10f;
    [SerializeField] private float minOrbit = 0.6f;
    [SerializeField] private float maxOrbitToCheck = 10f;

    private float currentXAngle;
    private float currentYAngle;
    private RaycastHit obstacleHit;

    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";
    private const float FullCircle = 360f;
    private const int Negative = -1;
    private const int Zero = 0;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        if (targetToFollow)
        {
            //Get Camera Angle by X Axis
            currentXAngle += Input.GetAxis(MouseXAxisName) * speedX * Time.deltaTime;
            currentXAngle = (currentXAngle + FullCircle) % FullCircle;

            //Get Camera Angle by Y Axis, Opposite Direction
            currentYAngle += Input.GetAxis(MouseYAxisName) * speedY * Time.deltaTime * Negative;
            currentYAngle = Mathf.Clamp(currentYAngle, minY, maxY);

            //Turn Camera to Result Angle
            transform.rotation = Quaternion.Euler(currentYAngle, currentXAngle, Zero);

            //Check Obstruction
            float resultOrbit = orbit;
            if (Physics.Raycast(targetToFollow.position, -transform.forward, out obstacleHit, maxOrbitToCheck, obstacleLayerMask))
            {
                resultOrbit = Mathf.Max(obstacleHit.distance, minOrbit);
            }
            //Perform Orbit Distance
            transform.position = targetToFollow.position - (transform.forward * resultOrbit);
        }
    }
}