using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterAudioTrigger : MonoBehaviour
{
    [SerializeField]
    private float waterLevelY = -2.0f; // Adjust this value to your desired water level threshold

    private bool isUnderwater = false;

    void Update()
    {
        // Check if the player's Y position is below the water level
        if (transform.position.y < waterLevelY)
        {
            if (!isUnderwater)
            {
                // Player has just moved underwater
                isUnderwater = true;
                WaterAudioControllerSingleton.Instance?.PlayWaterAudio();
            }
        }
        else
        {
            if (isUnderwater)
            {
                // Player has just moved above water
                isUnderwater = false;
                WaterAudioControllerSingleton.Instance?.StopWaterAudio();
            }
        }
    }
}
