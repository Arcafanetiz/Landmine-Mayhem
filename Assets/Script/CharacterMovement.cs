using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    [Header("Attach GameObject")]
    [SerializeField] private Transform mainCamera;
    [Header("Speed Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    private Rigidbody characterRigid;

    private const string MoveForwardAxisName = "Vertical";
    private const string MoveStrafeAxisName = "Horizontal";
    private const int ClampLimit = 1;
    private const int Zero = 0;
    private void Start()
    {
        characterRigid = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        DoMovement();
    }
    private void DoMovement()
    {
        Vector3 inputDirection = Vector3.zero;

        //Normalize camera pitch relative
        Vector3 cameraForward = mainCamera.forward;
        Vector3 cameraRight = mainCamera.right;
        cameraForward.y = Zero;
        cameraRight.y = Zero;
        cameraForward.Normalize();
        cameraRight.Normalize();

        //Do input direction
        inputDirection += Input.GetAxis(MoveForwardAxisName) * cameraForward;
        inputDirection += Input.GetAxis(MoveStrafeAxisName) * cameraRight;
        //Clamp direction to 1, prevent Diagonal Overshooting
        inputDirection = Vector3.ClampMagnitude(inputDirection, ClampLimit);

        //Turn character relative to camera, at 0 pitch plane
        if (inputDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(inputDirection);

            lookRotation.x = Zero;
            lookRotation.z = Zero;

            characterRigid.rotation = Quaternion.RotateTowards(characterRigid.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
        }
        //Move character towards input direction
        characterRigid.velocity = inputDirection * moveSpeed;
    }
}