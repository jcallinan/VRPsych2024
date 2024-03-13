using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSubmarineController : MonoBehaviour
{
    public CharacterController characterController;
    public float maxHeight = 10f; // Maximum height the player can move to
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Moves the CharacterController up and down, ensuring it does not go above maxHeight.
    /// </summary>
    public void MoveUpDown(VRLever _lever, float _movement, float _lastValue)
    {
        if (_lever != null && _lever.Value != _lastValue && characterController != null)
        {
            Vector3 movement = transform.up * _movement;
            movement.x = 0f; // Exclude horizontal movement
            movement.z = 0f; // Exclude depth movement

            // Predict the next position of the player
            Vector3 nextPosition = characterController.transform.position + movement;

            // Limit the y position to maxHeight
            if (nextPosition.y > maxHeight)
            {
                // Adjust movement so that it stops exactly at maxHeight
                float difference = maxHeight - characterController.transform.position.y;
                movement = transform.up * difference;
            }

          //  characterController.Move(movement);
        }
    }

    /// <summary>
    /// Moves the CharacterController left and right.
    /// </summary>
    public void MoveLeftRight(VRLever _lever, float _movement, float _lastValue)
    {
        if (_lever != null && _lever.Value != _lastValue && characterController != null)
        {
            Vector3 movement = transform.right * _movement;
            movement.y = 0f; // Exclude vertical movement
            movement.z = 0f; // Exclude depth movement

          //  characterController.Move(movement);
        }
    }
}
