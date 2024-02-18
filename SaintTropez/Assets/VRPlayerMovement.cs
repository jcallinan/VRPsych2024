using UnityEngine;
using Valve.VR;

public class VRPlayerMovement : MonoBehaviour
{
    public SteamVR_Action_Vector2 moveAction;
    public float speed = 3.0f;

    private CharacterController characterController;
    private Transform vrCameraTransform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        vrCameraTransform = Camera.main.transform; // Assuming the main camera is the VR headset
    }

    void Update()
    {
        Vector2 input = moveAction.GetAxis(SteamVR_Input_Sources.Any);
        // Use the camera's forward direction as the basis for movement direction
        Vector3 forward = vrCameraTransform.forward;
        Vector3 right = vrCameraTransform.right;

        // The input.y component affects movement in the camera's forward direction (including up and down)
        // The input.x component affects movement in the camera's right direction (horizontal)
        Vector3 direction = (forward * input.y + right * input.x).normalized;

        // Apply the calculated direction to the character controller
        // No need to project on plane since we want to include vertical movement
        characterController.Move(direction * speed * Time.deltaTime);
    }
}
