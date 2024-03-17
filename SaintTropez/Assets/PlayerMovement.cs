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
    public GameObject horizontalLever;
    public GameObject verticalLever;
    public AudioSource waterSound; // Assign this in the inspector
    public AudioSource aboveWaterSound;
    public AudioSource engineAudio; // Engine sound AudioSource

    // Rigidbody to be assigned in the inspector
    public Rigidbody rb;
    // Multipliers for movement speed
    public float horizontalSpeedMultiplier = 5.0f;
    public float verticalSpeedMultiplier = 5.0f;

    // Water sound activation depth
    private float waterActivationDepth = 6.5f;
    // Reference to the VR headset camera
    public Transform headsetTransform;

    public float rotationSpeed = 30.0f; // Degrees per second for rotation

    private float currentYaw = 0.0f;
    private float currentPitch = 0.0f;
    // This can be done in the Start() method
    CharacterController characterController;
    // Update is called once per frame
    public void Update()
    {
        MovePlayer();
        //  RotatePlayerBasedOnHeadset();
      // RotatePlayer();
        CheckAndPlayWaterSounds();



    }
    public void Start()
    {// Ensure the CharacterController component is retrieved only once for efficiency
        // This can be done in the Start() method
         characterController = player.GetComponent<CharacterController>();
    }
    private void MovePlayer()
    {
        

        if (characterController == null)
        {
            Debug.LogError("CharacterController is not found on the player object.");
            return;
        }

        // Assuming IsGrabbed is a property that correctly reflects the lever's state
        bool horizontalLeverGrabbed = horizontalLever.GetComponent<LinearDrive>().IsGrabbed;
        bool verticalLeverGrabbed = verticalLever.GetComponent<LinearDrive>().IsGrabbed;

        if (horizontalLeverGrabbed || verticalLeverGrabbed)
        {
            float horizontalMove = (horizontalMapping.value - 0.5f) * 2f * horizontalSpeedMultiplier;
            float verticalMove = (verticalMapping.value - 0.5f) * 2f * verticalSpeedMultiplier;

            // Calculate movement direction based on player's orientation
            Vector3 moveDirection = player.transform.right * horizontalMove + player.transform.forward * verticalMove;

            // Apply the movement through the CharacterController
            characterController.Move(moveDirection * Time.deltaTime);

            // Here, you might adjust the engine sound volume or other effects based on movement
            engineAudio.volume = Mathf.Clamp(moveDirection.magnitude, 0, 1); // Adjust as necessary
        }
        else
        {
            // Optionally, handle the case when levers are not grabbed (e.g., reduce sound volume)
            engineAudio.volume = Mathf.Lerp(engineAudio.volume, 0, Time.deltaTime * 2); // Smoothly reduce volume
        }
    }

    private void CheckAndPlayWaterSounds()
    {
        // Check the player's Y-axis position
        if (player.transform.position.y < waterActivationDepth)
        {
            // If below the activation depth and the water sound is not playing, play it
            if (!waterSound.isPlaying)
            {
                waterSound.Play();
                aboveWaterSound.Stop();
            }
        }
        else
        {
            // If above the activation depth and the water sound is playing, stop it
            if (waterSound.isPlaying)
            {
                waterSound.Stop();
                aboveWaterSound.Play();

            }
        }
    }
    private void MovePlayerGrabbingOld2()
    {
        // Get LinearDrive components from GameObjects
        LinearDrive horizontalLeverDrive = horizontalLever.GetComponent<LinearDrive>();
        LinearDrive verticalLeverDrive = verticalLever.GetComponent<LinearDrive>();

        // Check if either lever is being grabbed
        if (horizontalLeverDrive.IsGrabbed || verticalLeverDrive.IsGrabbed)
        {
            float horizontalMove = (horizontalMapping.value - 0.5f) * 2f * horizontalSpeedMultiplier;
            float verticalMove = (verticalMapping.value - 0.5f) * 2f * verticalSpeedMultiplier;

            // Calculate movement direction based on player's facing direction
            Vector3 moveDirection = player.transform.right * horizontalMove + player.transform.forward * verticalMove;
            moveDirection.y = 0; // Optionally, prevent movement in the vertical (Y) direction

            // Apply the movement
            player.transform.Translate(moveDirection * Time.deltaTime, Space.World);
        }
    }

    private void MovePlayerGrabbingOld()
    {
        // Get LinearDrive components from GameObjects
        LinearDrive horizontalLeverDrive = horizontalLever.GetComponent<LinearDrive>();
        LinearDrive verticalLeverDrive = verticalLever.GetComponent<LinearDrive>();

        // Check if either lever is being grabbed
        if (horizontalLeverDrive.IsGrabbed || verticalLeverDrive.IsGrabbed)
        {
            // Your existing movement code
            float horizontalMove = (horizontalMapping.value - 0.5f) * 2f * horizontalSpeedMultiplier;
            player.transform.Translate(Vector3.right * horizontalMove * Time.deltaTime);

            float verticalMove = (verticalMapping.value - 0.5f) * 2f * verticalSpeedMultiplier;
            player.transform.Translate(Vector3.forward * verticalMove * Time.deltaTime);
        }
    }


    private void MovePlayerOld()
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