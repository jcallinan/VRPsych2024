using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRGripper : MonoBehaviour
{
    public SteamVR_Behaviour_Pose CurrentController; // Attached via the inspector
    public AudioSource CurrentAudio;

    private static List<SteamVR_Behaviour_Pose> ControllerList = new List<SteamVR_Behaviour_Pose>();
    public SteamVR_Action_Vibration hapticAction = SteamVR_Actions.default_Haptic; // Haptic action defined in SteamVR Input

    public SteamVR_Action_Boolean GripAction = SteamVR_Actions.default_GrabGrip; // Adjust with your actual grip action
    public SteamVR_Action_Vibration HapticAction = SteamVR_Actions.default_Haptic; // Haptic feedback action

    private bool isColliding = false;
    private bool isGripping = false;
    public float VibrationLength = 0.1f;
    public float HapticPulseStrength = 0.5f;

    void OnEnable()
    {
        CurrentController = GetComponent<SteamVR_Behaviour_Pose>();
        CurrentAudio = GetComponent<AudioSource>();

        if (!ControllerList.Contains(CurrentController))
            ControllerList.Add(CurrentController);
    }

    void OnDisable()
    {
        if (ControllerList.Contains(CurrentController))
            ControllerList.Remove(CurrentController);
    }

    void OnCollisionEnter(Collision _collision)
    {
        if (!isColliding && !isGripping)
        {
            StartCoroutine(LongVibration(HapticPulseStrength, VibrationLength));
            CurrentAudio.Play();
            isColliding = true;
        }
    }

    IEnumerator LongVibration(float strength, float duration)
    {
        if (isGripping) yield break;

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            HapticAction.Execute(0, VibrationLength, 1 / VibrationLength * strength, HapticPulseStrength, CurrentController.inputSource);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void OnCollisionExit(Collision _collision)
    {
        isColliding = false;
    }

    public static List<SteamVR_Behaviour_Pose> GetControllers()
    {
        return new List<SteamVR_Behaviour_Pose>(ControllerList);
    }
    /// <summary>
    /// Initiates a haptic vibration on the controller.
    /// </summary>
    /// <param name="duration">Duration of the vibration in seconds.</param>
    /// <param name="strength">Strength of the vibration (0-1).</param>
    public void HapticVibration(float duration, float strength)
    {
        // Ensure there's a controller attached
        if (CurrentController != null && CurrentController.isActiveAndEnabled)
        {
            // Execute the haptic action
            hapticAction.Execute(0, duration, 1.0f / duration, strength, CurrentController.inputSource);
        }
    }
}
