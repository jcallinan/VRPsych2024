using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SubmarineMover : MonoBehaviour
{
    public Transform Joystick;
    public float joyMove = 0.1f;

    public SteamVR_Action_Vector2 moveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("platformer", "Move");
    public SteamVR_Action_Boolean jumpAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("platformer", "Jump");

    public CharacterController characterController; // Reference to the CharacterController

    public Renderer jumpHighlight;

    private Vector3 movement;
    private bool jumpRequest = false;
    private float verticalSpeed = 0f; // Vertical speed for jumping/gravity
    private float gravity = -9.81f; // Gravity effect
    private float jumpSpeed = 5f; // Initial speed for jumps
    private float glow;
    private SteamVR_Input_Sources hand;
    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
       // characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found. Please attach a CharacterController to the player GameObject.");
        }
    }

    private void Update()
    {
        if (interactable.attachedToHand)
        {
            hand = interactable.attachedToHand.handType;
            Vector2 m = moveAction[hand].axis;
            movement = new Vector3(m.x, 0, m.y) * joyMove;

            if (jumpAction[hand].stateDown && characterController.isGrounded)
            {
                jumpRequest = true;
            }

            glow = Mathf.Lerp(glow, jumpAction[hand].state ? 1.5f : 1.0f, Time.deltaTime * 20);
        }
        else
        {
            movement = Vector3.zero;
            jumpRequest = false;
            glow = 0;
        }

        // Optional: Adjust jumpHighlight based on jump state
        // jumpHighlight.sharedMaterial.SetColor("_EmissionColor", Color.white * glow);
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            verticalSpeed = 0; // Reset vertical speed on ground
            if (jumpRequest)
            {
                verticalSpeed = jumpSpeed; // Apply jump speed
                jumpRequest = false;
            }
        }
        else
        {
            verticalSpeed += gravity * Time.fixedDeltaTime; // Apply gravity
        }

        Vector3 finalMovement = movement + Vector3.up * verticalSpeed;
        characterController.Move(finalMovement * Time.fixedDeltaTime);
    }
}
