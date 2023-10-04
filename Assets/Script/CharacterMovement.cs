using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    public Transform cameraTransform;
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

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        inputDirection += Input.GetAxis("Vertical") * cameraForward;
        inputDirection += Input.GetAxis("Horizontal") * cameraRight;

        inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

        if (inputDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(inputDirection);

            lookRotation.x = 0f;
            lookRotation.z = 0f;

            characterRigid.rotation = Quaternion.RotateTowards(characterRigid.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
        }

        characterRigid.velocity = inputDirection * moveSpeed;
    }
}