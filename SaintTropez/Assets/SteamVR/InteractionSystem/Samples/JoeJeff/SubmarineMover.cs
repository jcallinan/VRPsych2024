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

    // Reference to the VR headset Transform for reading head tilt.
    public Transform vrHeadset;

    private Vector3 movement;
    public float verticalSpeed = 0f;
    public float gravity = -9.81f; // Gravity effect on the player
    public float jumpSpeed = 5f; // Speed at which the player will "jump" or move upwards
    public float moveSpeed = 5f; // Movement speed based on joystick input
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

            // Adjust glow based on jump action state for visual feedback
            glow = Mathf.Lerp(glow, jumpAction[hand].state ? 1.5f : 1.0f, Time.deltaTime * 20);

            // Adjust joystick visual representation based on input
            Joystick.localPosition = new Vector3(joystickInput.x, 0, joystickInput.y) * joyMove;

            // Check for jump action and apply vertical movement if triggered
            if (jumpAction.GetStateDown(hand))
            {
                verticalSpeed = jumpSpeed;
            }
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
    }

    private void HandleMovement()
    {
        // Handle horizontal movement based on joystick input
        Vector3 horizontalMovement = transform.TransformDirection(movement) * moveSpeed;

        // Apply gravity continuously, but allow "jumping" or swimming upwards anytime
        if (!characterController.isGrounded)
        {
            verticalSpeed += gravity * Time.fixedDeltaTime;
        }

        // Prevent accumulating negative vertical speed indefinitely when grounded
        if (characterController.isGrounded && verticalSpeed < 0)
        {
            verticalSpeed = 0;
        }

        // Combine horizontal movement with vertical movement (jumping/swimming and gravity)
        Vector3 finalMovement = horizontalMovement + Vector3.up * verticalSpeed;
        characterController.Move(finalMovement * Time.fixedDeltaTime);
    }
}
