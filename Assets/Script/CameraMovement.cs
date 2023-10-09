using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    [Header("Insert")]
    public Transform targetTransform;
    public LayerMask obstacleLayerMask;

    [Header("Turn Speed Settings")]
    public float speedX = 200f;
    public float speedY = 200f;
    [Header("Pitch Settings")]
    public float minY = 60f;
    public float maxY = 80f;
    [Header("Orbit Distance Settings")]
    public float orbitDistance = 5f;
    public float minOrbitDistance = 0.6f;

    private float currentXAngle;
    private float currentYAngle;

    private RaycastHit obstacleHit;

    private readonly float FULLCIRCLE = 360f;
    private readonly float MAXORBITDISTANCECHECK = 10f;
    private readonly int NEGATE = -1;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        if (targetTransform != null)
        {
            //Get Camera Angle by X Axis
            currentXAngle += Input.GetAxis("Mouse X") * speedX * Time.deltaTime;
            currentXAngle = (currentXAngle + FULLCIRCLE) % FULLCIRCLE;

            //Get Camera Angle by Y Axis, Opposite Direction
            currentYAngle += Input.GetAxis("Mouse Y") * speedY * Time.deltaTime * NEGATE;
            currentYAngle = Mathf.Clamp(currentYAngle, minY, maxY);

            //Turn Camera to Result Angle
            transform.rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0);

            //Check Obstruction
            float resultOrbit = orbitDistance;
            if (Physics.Raycast(transform.position, -transform.forward, out obstacleHit, MAXORBITDISTANCECHECK, obstacleLayerMask))
            {
                resultOrbit = obstacleHit.distance;
            }
            //Perform Orbit Distance
            transform.position = targetTransform.position - (transform.forward * resultOrbit);
        }
    }
}