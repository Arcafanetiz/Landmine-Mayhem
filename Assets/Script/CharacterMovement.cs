using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private Animator animator; // Character Animator

    // Speed Settings
    [Header("Speed Settings")]
    [SerializeField] private float moveSpeed;   // Characgter Move Speed
    [SerializeField] private float turnSpeed;   // Characgter Turn Speed

    // Reference Variables
    private Transform mainCamTransform;
    private Rigidbody characterRigid;

    // Constant Variables
    private const string MoveForwardAxisName = "Vertical";
    private const string MoveStrafeAxisName = "Horizontal";
    private const int ClampLimit = 1;
    private const int Zero = 0;

    private void Awake()
    {
        characterRigid = GetComponent<Rigidbody>();
        mainCamTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        Vector3 inputDirection = Vector3.zero;

        //Normalize camera pitch relative
        Vector3 cameraForward = mainCamTransform.forward;
        Vector3 cameraRight = mainCamTransform.right;
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
        animator.SetFloat("Velocity", characterRigid.velocity.magnitude);
    }
}