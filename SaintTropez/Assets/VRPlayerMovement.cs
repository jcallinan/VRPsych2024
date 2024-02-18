using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRPlayerMovement : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 2.0f;

    private CharacterController characterController;
    private Transform cameraRigTransform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController is not attached to VR Player");
        }

        cameraRigTransform = SteamVR_Render.Top().head.transform;
    }

    void Update()
    {
        HandleHead();
        HandleHeight();
        CalculateMovement();
    }

    private void HandleHead()
    {
        // Ensure the head is always aligned with the camera rig
        Vector3 headPosition = cameraRigTransform.position;
        headPosition.y = transform.position.y;
        transform.position = headPosition;
    }

    private void HandleHeight()
    {
        // Adjust the height based on the head's position
        float headHeight = Mathf.Clamp(cameraRigTransform.localPosition.y, 1, 2);
        characterController.height = headHeight;

        // Cut in half, add skin
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        // Move capsule in local space
        newCenter.x = cameraRigTransform.localPosition.x;
        newCenter.z = cameraRigTransform.localPosition.z;

        // Apply
        characterController.center = newCenter;
    }

    private void CalculateMovement()
    {
        // Get input
        Vector2 movement = input.axis;
        Vector3 direction = Vector3.zero;
        direction.x = movement.x;
        direction.z = movement.y;

        // Transform direction to be relative to camera
        direction = cameraRigTransform.TransformDirection(direction);

        // Move
        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));
    }
}
