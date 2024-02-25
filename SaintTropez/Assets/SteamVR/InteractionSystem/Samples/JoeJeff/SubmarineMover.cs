using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SubmarineMover : MonoBehaviour
{
    public Transform Joystick;
    public float joyMove = 0.1f; // Adjust this to scale movement sensitivity

    public SteamVR_Action_Vector2 moveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("platformer", "Move");
    public SteamVR_Action_Boolean jumpAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("platformer", "Jump");

    public CharacterController characterController;

    public Renderer jumpHighlight;

    private Vector3 movement;
    private float verticalSpeed = 0f;
    private float gravity = -9.81f;
    private float jumpSpeed = 5f;
    private float moveSpeed = 5f; // New variable to control forward/backward movement speed
    private float glow;
    private SteamVR_Input_Sources hand;
    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if (interactable.attachedToHand)
        {
            hand = interactable.attachedToHand.handType;
            Vector2 joystickInput = moveAction[hand].axis;
            movement = new Vector3(joystickInput.x, 0, joystickInput.y);

            // Adjust glow based on jump action state (for visual feedback, if needed)
            glow = Mathf.Lerp(glow, jumpAction[hand].state ? 1.5f : 1.0f, Time.deltaTime * 20);

            // Optionally adjust joystick visual representation
            Joystick.localPosition = new Vector3(joystickInput.x, 0, joystickInput.y) * joyMove;
        }
        else
        {
            movement = Vector3.zero;
            glow = 0;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJumpAndGravity();
    }

    private void HandleMovement()
    {
        // Convert local movement direction to world space
        Vector3 worldMovement = transform.TransformDirection(movement) * moveSpeed;
        characterController.Move(worldMovement * Time.fixedDeltaTime);
    }

    private void HandleJumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            verticalSpeed = 0; // Reset vertical speed on ground
            if (jumpAction.GetStateDown(hand)) // Check for jump input
            {
                verticalSpeed = jumpSpeed; // Apply jump speed
            }
        }
        else
        {
            verticalSpeed += gravity * Time.fixedDeltaTime; // Apply gravity
        }

        // Apply vertical movement (jumping and gravity)
        characterController.Move(Vector3.up * verticalSpeed * Time.fixedDeltaTime);
    }
}
