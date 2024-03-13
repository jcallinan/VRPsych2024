using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    // Assign these in the inspector
    public LinearMapping horizontalMapping;
    public LinearMapping verticalMapping;
    public GameObject player;

    // Multipliers for movement speed
    public float horizontalSpeedMultiplier = 5.0f;
    public float verticalSpeedMultiplier = 5.0f;

    // Reference to the VR headset camera
    public Transform headsetTransform;

    public float rotationSpeed = 30.0f; // Degrees per second for rotation

    private float currentYaw = 0.0f;
    private float currentPitch = 0.0f;

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        //  RotatePlayerBasedOnHeadset();
        RotatePlayer();



    }

    private void MovePlayerO()
    {
        // Assuming value ranges from 0 to 1
        // Mapping the value to a range that suits your game
        // You might need to adjust the multipliers for speed and direction

        // For horizontal movement (left and right)
        float horizontalMove = (horizontalMapping.value - 0.5f) * 2f * horizontalSpeedMultiplier; // Converts 0->1 to -1->1 and applies speed multiplier
        player.transform.Translate(Vector3.right * horizontalMove * Time.deltaTime);

        // For vertical movement (forward and backward)
        float verticalMove = (verticalMapping.value - 0.5f) * 2f * verticalSpeedMultiplier; // Converts 0->1 to -1->1 and applies speed multiplier
        player.transform.Translate(Vector3.forward * verticalMove * Time.deltaTime);
    }
    private void MovePlayerWIthRotateWrong()
    {
        float horizontalMove = (horizontalMapping.value - 0.5f) * 2f * horizontalSpeedMultiplier; // Converts 0->1 to -1->1 and applies speed multiplier
        float verticalMove = (verticalMapping.value - 0.5f) * 2f * verticalSpeedMultiplier; // Converts 0->1 to -1->1 and applies speed multiplier

        // Apply movement in world space
        player.transform.Translate(Vector3.right * horizontalMove * Time.deltaTime, Space.World);
        player.transform.Translate(Vector3.forward * verticalMove * Time.deltaTime, Space.World);
    }

    private void RotatePlayerBasedOnHeadset()
    {
        // Get the yaw (Y axis) rotation of the headset
        float headsetYaw = headsetTransform.rotation.eulerAngles.y;

        // Determine the target rotation for the player
        Quaternion targetRotation = Quaternion.Euler(0, headsetYaw, 0);

        // Smoothly interpolate the player's rotation towards the target rotation
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.deltaTime * 10); // Adjust the multiplier for smoothing speed
    }
    private void RotatePlayer()
    {
        // Calculate target yaw and pitch from LinearMapping values
        float targetYaw = (horizontalMapping.value - 0.5f) * 360f; // Full circle based on mapping
        float targetPitch = (verticalMapping.value - 0.5f) * 180f; // Half circle for up/down

        // Smoothly interpolate current yaw and pitch towards target values
        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * rotationSpeed);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * rotationSpeed);

        // Clamp pitch to prevent flipping over, adjust the range as needed
        currentPitch = Mathf.Clamp(currentPitch, -90f, 90f);

        // Apply the calculated rotation to the player
        player.transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
    }

}