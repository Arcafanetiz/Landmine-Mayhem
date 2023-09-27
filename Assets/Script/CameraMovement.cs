using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    public Transform targetTransform;
    public LayerMask obstacleLayerMask;

    public float speedX = 200f;
    public float speedY = 200f;
    public float minY = 60f;
    public float maxY = 80f;

    public float orbitDistance = 5f;
    public float minOrbitDistance = 0.6f;

    private float currentXAngle;
    private float currentYAngle;

    private RaycastHit obstacleHit;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        if (targetTransform != null)
        {
            currentXAngle += Input.GetAxis("Mouse X") * speedX * Time.deltaTime;
            currentXAngle = (currentXAngle + 360f) % 360f;

            currentYAngle += Input.GetAxis("Mouse Y") * speedY * Time.deltaTime * -1f;
            currentYAngle = Mathf.Clamp(currentYAngle, minY, maxY);

            transform.rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0f);
            float resultOrbit = orbitDistance;

            if (Physics.Raycast(transform.position, -transform.forward, out obstacleHit, 10, obstacleLayerMask))
            {
                resultOrbit = obstacleHit.distance;
            }
            transform.position = targetTransform.position - (transform.forward * resultOrbit);
        }
    }
}