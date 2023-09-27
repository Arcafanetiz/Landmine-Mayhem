using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed;
    public float turnSpeed;

    private Vector3 turnDirection;
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
        turnDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            turnDirection += cameraTransform.forward;
            DoTurn();
            characterRigid.velocity = transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            turnDirection += -cameraTransform.forward;
            DoTurn();
            characterRigid.velocity = transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnDirection += -cameraTransform.right;
            DoTurn();
            characterRigid.velocity = transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnDirection += cameraTransform.right;
            DoTurn();
            characterRigid.velocity = transform.forward * moveSpeed;
        }
    }
    private void DoTurn()
    {
        Vector3 forward = transform.forward;
        if (turnDirection.magnitude > 0f)
        {
            forward = Vector3.ProjectOnPlane(turnDirection, Vector3.up);
        }
        Quaternion turnTo = Quaternion.LookRotation(forward, Vector3.up);

        characterRigid.transform.rotation = Quaternion.RotateTowards(transform.rotation, turnTo, turnSpeed * Time.deltaTime);
    }
}