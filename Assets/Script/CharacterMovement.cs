using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    [Header("Insert")]
    public Transform cameraTransform;
    [Header("Speed Settings")]
    public float moveSpeed;
    public float turnSpeed;

    private Rigidbody characterRigid;
    void Start()
    {
        characterRigid = gameObject.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        DoMovement();
    }
    private void DoMovement()
    {
        Vector3 inputDirection = Vector3.zero;

        //Normalize camera pitch relative
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        //Do input direction
        inputDirection += Input.GetAxis("Vertical") * cameraForward;
        inputDirection += Input.GetAxis("Horizontal") * cameraRight;
        //Clamp direction to 1, prevent Diagonal Overshooting
        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        //Turn character relative to camera, at 0 pitch plane
        if (inputDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(inputDirection);

            lookRotation.x = 0f;
            lookRotation.z = 0f;

            characterRigid.rotation = Quaternion.RotateTowards(characterRigid.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
        }
        //Move character towards input direction
        characterRigid.velocity = inputDirection * moveSpeed;
    }
}