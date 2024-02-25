using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public Transform headTransform; // Assign your VR Camera (Head) Transform here

    // Speed scaling factors
    public float forwardBackwardSpeedScale = 0.1f;
    public float upDownSpeedScale = 0.05f;

    private void Start()
    {
        if (headTransform == null)
        {
            Debug.LogError("Head Transform is not assigned. Please assign the VR Camera (Head) Transform in the inspector.");
        }
    }

    // Function to be called for moving forward and backward
    // Value range from -60 to 60, where negative values mean backward and positive values mean forward
    public void MoveForwardBackward(float value)
    {
        // Normalize the value to be between -1 and 1
        float normalizedValue = value / 60.0f;
        // Calculate the direction and magnitude
        Vector3 moveDirection = headTransform.forward * normalizedValue * forwardBackwardSpeedScale;
        // Adjust position
        transform.position += moveDirection * Time.deltaTime;
    }

    // Function to be called for moving up and down
    // Value range from -60 to 60, where negative values mean down and positive values mean up
    public void MoveUpDown(float value)
    {
        // Normalize the value to be between -1 and 1
        float normalizedValue = value / 60.0f;
        // Calculate the direction and magnitude
        Vector3 moveDirection = Vector3.up * normalizedValue * upDownSpeedScale;
        // Adjust position
        transform.position += moveDirection * Time.deltaTime;
    }

    private void Update()
    {
        // Example usage (normally, you would call these from your lever input handling code)
        // MoveForwardBackward(30); // Move forward
        // MoveUpDown(-30); // Move down
    }
}
